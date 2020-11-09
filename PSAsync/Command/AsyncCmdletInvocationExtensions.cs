using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync.Command
{
    public static class AsyncCmdletInvocationExtensions
    {
        [return: MaybeNull]
        public static TResult ExecuteAsyncAction<TCmdlet, TArgument, TResult>(
            this TCmdlet cmdlet,
            Func<TCmdlet, TArgument, CancellationToken, Task<TResult>> action,
            TArgument argument)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));
            Requires.NotNull(action, nameof(action));

            var result = AsyncMethodRunner.ExecuteAsyncMethod(
                cmdlet,
                action,
                argument);

            return result;
        }

        [return: MaybeNull]
        public static TResult ExecuteAsyncAction<TCmdlet, TResult>(
            this TCmdlet cmdlet,
            Func<TCmdlet, CancellationToken, Task<TResult>> action)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));
            Requires.NotNull(action, nameof(action));

            var result = AsyncMethodRunner.ExecuteAsyncMethod(
                cmdlet,
                (p, _, c) => action(p, c),
                Unit.Instance);

            return result;
        }

        public static void ExecuteAsyncAction<TCmdlet, TArgument>(
            this TCmdlet cmdlet,
            Func<TCmdlet, TArgument, CancellationToken, Task> action,
            TArgument argument)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));
            Requires.NotNull(action, nameof(action));

            AsyncMethodRunner.ExecuteAsyncMethod(
                cmdlet,
                async (p, a, c) => {
                    await action(p, a, c).ConfigureAwait(false);
                    return Unit.Instance;
                },
                argument);
        }

        public static void ExecuteAsyncAction<TCmdlet>(
            this TCmdlet cmdlet,
            Func<TCmdlet, CancellationToken, Task> action)
            where TCmdlet :
                Cmdlet,
                IAsyncCmdlet
        {
            Requires.NotNull(cmdlet, nameof(cmdlet));
            Requires.NotNull(action, nameof(action));

            AsyncMethodRunner.ExecuteAsyncMethod(
                cmdlet,
                async (p, _, c) => {
                    await action(p, c).ConfigureAwait(false);
                    return Unit.Instance;
                },
                Unit.Instance);
        }
    }
}
