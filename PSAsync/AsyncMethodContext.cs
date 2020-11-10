using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync
{
    internal sealed class AsyncMethodContext :
        IDisposable
    {
        public AsyncMethodContext(
            object associatedObject)
        {
            this._associatedObject = associatedObject;

            this._mainThreadId = Thread.CurrentThread.ManagedThreadId;

            this._queue = new BlockingCollection<Action>();

            this._cts = new CancellationTokenSource();
        }

        public static AsyncMethodContext Start(
            object associatedObject)
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            var context = _contexts.GetOrAdd(
                associatedObject,
                x => new AsyncMethodContext(x));

            return context;
        }

        public static AsyncMethodContext GetContext(
            object associatedObject)
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            if (!TryGetContext(associatedObject, out var context))
            {
                throw new InvalidOperationException();
            }

            return context;
        }

        public static bool TryGetContext(
            object associatedObject,

            [MaybeNullWhen(false)]
            out AsyncMethodContext context)
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            context = null!;

            if (!_contexts.TryGetValue(associatedObject, out var ctx))
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

        public AwaitableAction<object, TArgument, TResult> CreateAction<TArgument, TResult>(
            Func<object, TArgument, AsyncMethodContext, TResult> action,
            TArgument argument,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this._cts.Token);

            return new AwaitableAction<object, TArgument, TResult>(
                this._associatedObject,
                (c, a) => action(c, a, this),
                argument,
                state => ((CancellationTokenSource?)state)!.Dispose(),
                linkedTokenSource,
                linkedTokenSource.Token);
        }

        public Task<TResult> QueueAction<TArgument, TResult>(
            Func<object, TArgument, AsyncMethodContext, TResult> action,
            TArgument argument,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            var awaitableAction = this.CreateAction(
                action,
                argument,
                cancellationToken);

            awaitableAction.Task.ContinueWith(
                t =>
                {
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

        public Task QueueAction<TArgument>(
            Action<object, TArgument, AsyncMethodContext> action,
            TArgument argument,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            return this.QueueAction(
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

            _contexts.Remove(this._associatedObject, out _);
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

        internal ShouldContinueContext ShouldContinueContext
        {
            [DebuggerStepThrough]
            get;

            [DebuggerStepThrough]
            set;
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
                throw new ObjectDisposedException(nameof(AsyncMethodContext));
            }
        }
        
        private static readonly ConcurrentDictionary<object, AsyncMethodContext> _contexts =
            new ConcurrentDictionary<object, AsyncMethodContext>();

        private readonly object _associatedObject;

        private readonly int _mainThreadId;

        private readonly BlockingCollection<Action> _queue;

        private readonly CancellationTokenSource _cts;

        private bool _disposed;
    }
}
