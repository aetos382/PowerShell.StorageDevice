using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsyncProvider
{
    internal static class AsyncMethodRunner
    {
        [return: MaybeNull]
        public static TResult ExecuteAsyncMethod<TProvider, TState, TResult>(
            TProvider provider,
            Func<TProvider, TState, CancellationToken, Task<TResult>> asyncMethod,
            TState state)
            where TProvider : CmdletProvider, IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(asyncMethod, nameof(asyncMethod));

            using var context = AsyncProviderContext.Start(provider);

            var task = asyncMethod(provider, state, context.GetCancellationToken());

            if (task is null)
            {
                throw new InvalidOperationException();
            }

            ExceptionDispatchInfo? exceptionDispatcher = null;

            TResult result = default;

            try
            {
                task.ContinueWith(
                    (t, state) => ((AsyncProviderContext)state)!.Close(),
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

                    if (pse != null)
                    {
                        exceptionDispatcher = ExceptionDispatchInfo.Capture(pse);
                    }
                    else if (exceptions.Any(e => !(e is OperationCanceledException)))
                    {
                        exceptionDispatcher = ExceptionDispatchInfo.Capture(ex);
                    }
                }
            }

            exceptionDispatcher?.Throw();

            return result;
        }
    }
}
