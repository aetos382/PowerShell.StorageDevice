using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsyncProvider
{
    internal sealed class AsyncProviderContext :
        IDisposable
    {
        private AsyncProviderContext(
            CmdletProvider provider)
        {
            if (!_contexts.TryAdd(provider, this))
            {
                throw new InvalidOperationException();
            }

            this._provider = provider;

            this._mainThreadId = Thread.CurrentThread.ManagedThreadId;

            this._queue = new BlockingCollection<Action>();

            this._cts = new CancellationTokenSource();
        }

        public static AsyncProviderContext Start<TProvider>(
            TProvider provider)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            var context = new AsyncProviderContext(provider);
            return context;
        }

        public static AsyncProviderContext GetContext<TProvider>(
            TProvider provider)
            where TProvider : CmdletProvider, IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            if (!TryGetContext(provider, out var context))
            {
                throw new InvalidOperationException();
            }

            return context;
        }

        public static bool TryGetContext<TProvider>(
            TProvider provider,

            [MaybeNullWhen(false)]
            out AsyncProviderContext context)
            where TProvider : CmdletProvider, IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            context = null!;

            if (!_contexts.TryGetValue(provider, out var ctx))
            {
                return false;
            }

            if (ctx._disposed)
            {
                return false;
            }

            context = ctx;
            return true;
        }

        public AwaitableAction<TProvider, TArgument, TResult> CreateAction<TProvider, TArgument, TResult>(
            TProvider provider,
            Func<TProvider, TArgument, AsyncProviderContext, TResult> action,
            TArgument argument,
            CancellationToken cancellationToken)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this._cts.Token);

            return new AwaitableAction<TProvider, TArgument, TResult>(
                provider,
                (c, a) => action(c, a, this),
                argument,
                state => ((CancellationTokenSource?)state)!.Dispose(),
                linkedTokenSource,
                linkedTokenSource.Token);
        }

        public Task<TResult> QueueAction<TProvider, TArgument, TResult>(
            TProvider provider,
            Func<TProvider, TArgument, AsyncProviderContext, TResult> action,
            TArgument argument,
            CancellationToken cancellationToken)
            where TProvider : CmdletProvider, IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            var awaitableAction = this.CreateAction(
                provider,
                action,
                argument,
                cancellationToken);

            awaitableAction.Task.ContinueWith(
                t => {
                    bool pipelineStopped = t.Exception!
                        .Flatten()
                        .InnerExceptions
                        .Any(
                            e => e is PipelineStoppedException);

                    if (!pipelineStopped)
                    {
                        return;
                    }

                    this._cts.Cancel();
                },
                this._cts.Token,
                TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously,
                TaskScheduler.Current);

            if (this.IsMainThread)
            {
                awaitableAction.Invoke();
            }
            else
            {
                this._queue.Add(awaitableAction.Invoke, CancellationToken.None);
            }

            return awaitableAction.Task;
        }

        public Task QueueAction<TProvider, TArgument>(
            TProvider provider,
            Action<TProvider, TArgument, AsyncProviderContext> action,
            TArgument argument,
            CancellationToken cancellationToken)
            where TProvider : CmdletProvider, IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            return this.QueueAction(
                provider,
                (p, a, c) =>
                {
                    action(p, a, c);
                    return Unit.Instance;
                },
                argument,
                cancellationToken);
        }

        internal CancellationToken GetCancellationToken()
        {
            this.CheckDisposed();

            return this._cts.Token;
        }

        public void Dispose()
        {
            if (this._disposed)
            {
                return;
            }

            if (!this.IsMainThread)
            {
                throw new InvalidOperationException();
            }

            this._disposed = true;

            this._cts.Dispose();
            this._queue.Dispose();

            _contexts.Remove(this._provider, out _);
        }

        public IEnumerable<Action> GetActions()
        {
            this.CheckDisposed();

            return this._queue.GetConsumingEnumerable();
        }

        public void Close()
        {
            if (this._disposed)
            {
                return;
            }

            this._queue.CompleteAdding();
        }

        private bool IsMainThread
        {
            get
            {
                return Thread.CurrentThread.ManagedThreadId == this._mainThreadId;
            }
        }

        private void CheckDisposed()
        {
            if (this._disposed)
            {
                throw new ObjectDisposedException(nameof(AsyncProviderContext));
            }
        }

        private static readonly Dictionary<CmdletProvider, AsyncProviderContext> _contexts =
            new Dictionary<CmdletProvider, AsyncProviderContext>();

        private readonly CmdletProvider _provider;

        private readonly int _mainThreadId;

        private readonly BlockingCollection<Action> _queue;

        private readonly CancellationTokenSource _cts;

        private bool _disposed = false;
    }
}
