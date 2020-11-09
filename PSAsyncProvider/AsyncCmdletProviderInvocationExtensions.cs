using System;
using System.Diagnostics.CodeAnalysis;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsyncProvider
{
    public static class AsyncCmdletProviderInvocationExtensions
    {
        [return: MaybeNull]
        public static TResult ExecuteAsyncAction<TProvider, TArgument, TResult>(
            this TProvider provider,
            Func<TProvider, TArgument, CancellationToken, Task<TResult>> action,
            TArgument argument)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            var result = AsyncMethodRunner.ExecuteAsyncMethod(
                provider,
                action,
                argument);

            return result;
        }

        [return: MaybeNull]
        public static TResult ExecuteAsyncAction<TProvider, TResult>(
            this TProvider provider,
            Func<TProvider, CancellationToken, Task<TResult>> action)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            var result = AsyncMethodRunner.ExecuteAsyncMethod(
                provider,
                (p, _, c) => action(p, c),
                Unit.Instance);

            return result;
        }

        public static void ExecuteAsyncAction<TProvider, TArgument>(
            this TProvider provider,
            Func<TProvider, TArgument, CancellationToken, Task> action,
            TArgument argument)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            AsyncMethodRunner.ExecuteAsyncMethod(
                provider,
                async (p, a, c) => {
                    await action(p, a, c).ConfigureAwait(false);
                    return Unit.Instance;
                },
                argument);
        }

        public static void ExecuteAsyncAction<TProvider>(
            this TProvider provider,
            Func<TProvider, CancellationToken, Task> action)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));
            Requires.NotNull(action, nameof(action));

            AsyncMethodRunner.ExecuteAsyncMethod(
                provider,
                async (p, _, c) => {
                    await action(p, c).ConfigureAwait(false);
                    return Unit.Instance;
                },
                Unit.Instance);
        }
    }
}
