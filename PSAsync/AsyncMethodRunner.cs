using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync
{
    internal static class AsyncMethodRunner
    {
        [return: MaybeNull]
        public static TResult ExecuteAsyncMethod<TObject, TState, TResult>(
            TObject associatedObject,
            Func<TObject, TState, CancellationToken, Task<TResult>> asyncMethod,
            TState state)
            where TObject : class
        {
            Requires.NotNull(associatedObject, nameof(associatedObject));
            Requires.NotNull(asyncMethod, nameof(asyncMethod));

            using var context = AsyncMethodContext.Start(associatedObject);

            var task = asyncMethod(associatedObject, state, context.GetCancellationToken());

            if (task is null)
            {
                throw new InvalidOperationException();
            }

            ExceptionDispatchInfo? exceptionDispatcher = null;

            TResult result = default;

            try
            {
                task.ContinueWith(
                    (t, state) => ((AsyncMethodContext)state)!.Close(),
                    context,
                    CancellationToken.None,
                    TaskContinuationOptions.ExecuteSynchronously,
                    TaskScheduler.Current);

                foreach (var action in context.GetActions())
                {
                    action();
                }
            }
            finally
            {
                try
                {
                    result = task.GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                }
                catch (AggregateException ex)
                {
                    var exceptions = ex.Flatten().InnerExceptions;
                    var pse = exceptions.FirstOrDefault(e => e is PipelineStoppedException);

                    if (pse is not null)
                    {
                        exceptionDispatcher = ExceptionDispatchInfo.Capture(pse);
                    }
                    else if (exceptions.Any(e => !(e is OperationCanceledException)))
                    {
                        exceptionDispatcher = ExceptionDispatchInfo.Capture(ex);
                    }
                }
                catch (Exception ex)
                {
                    exceptionDispatcher = ExceptionDispatchInfo.Capture(ex);
                }
            }

            exceptionDispatcher?.Throw();

            return result;
        }
    }
}
