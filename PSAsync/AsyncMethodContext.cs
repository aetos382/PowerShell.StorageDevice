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
    internal static class AsyncMethodContext
    {
        public static AsyncMethodContext<TObject> Start<TObject>(
            TObject associatedObject)
            where TObject : class
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            return AsyncMethodContext<TObject>.Start(associatedObject);
        }

        public static AsyncMethodContext<TObject> GetContext<TObject>(
            TObject associatedObject)
            where TObject : class
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            return AsyncMethodContext<TObject>.GetContext(associatedObject);
        }
    }

    internal sealed class AsyncMethodContext<TObject> :
        IDisposable
        where TObject : class
    {
        private AsyncMethodContext(
            TObject associatedObject)
        {
            if (!_contexts.TryAdd(associatedObject, this))
            {
                throw new InvalidOperationException();
            }

            this._associatedObject = associatedObject;

            this._mainThreadId = Thread.CurrentThread.ManagedThreadId;

            this._queue = new BlockingCollection<Action>();

            this._cts = new CancellationTokenSource();
        }

        public static AsyncMethodContext<TObject> Start(
            TObject associatedObject)
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            var context = new AsyncMethodContext<TObject>(associatedObject);
            return context;
        }

        public static AsyncMethodContext<TObject> GetContext(
            TObject associatedObject)
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));

            if (!TryGetContext(associatedObject, out var context))
            {
                throw new InvalidOperationException();
            }

            return context;
        }

        public static bool TryGetContext(
            TObject associatedObject,

            [MaybeNullWhen(false)]
            out AsyncMethodContext<TObject> context)
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

        public AwaitableAction<TObject, TArgument, TResult> CreateAction<TArgument, TResult>(
            Func<TObject, TArgument, AsyncMethodContext<TObject>, TResult> action,
            TArgument argument,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(action, nameof(action));

            this.CheckDisposed();

            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, this._cts.Token);

            return new AwaitableAction<TObject, TArgument, TResult>(
                this._associatedObject,
                (c, a) => action(c, a, this),
                argument,
                state => ((CancellationTokenSource?)state)!.Dispose(),
                linkedTokenSource,
                linkedTokenSource.Token);
        }

        public Task<TResult> QueueAction<TArgument, TResult>(
            Func<TObject, TArgument, AsyncMethodContext<TObject>, TResult> action,
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

        public Task QueueAction<TArgument>(
            Action<TObject, TArgument, AsyncMethodContext<TObject>> action,
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

        private static readonly Dictionary<TObject, AsyncMethodContext<TObject>> _contexts =
            new Dictionary<TObject, AsyncMethodContext<TObject>>();

        private readonly TObject _associatedObject;

        private readonly int _mainThreadId;

        private readonly BlockingCollection<Action> _queue;

        private readonly CancellationTokenSource _cts;

        private bool _disposed;
    }
}
