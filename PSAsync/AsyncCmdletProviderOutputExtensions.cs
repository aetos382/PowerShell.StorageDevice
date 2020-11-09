using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync
{
    public static class AsyncCmdletProviderOutputExtensions
    {
        public static Task WriteItemObjectAsync<TProvider>(
            this TProvider provider,
            object item,
            string path,
            bool isContainer,
            CancellationToken cancellationToken)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            var context = AsyncMethodContext.GetContext(provider);

            var task = context.QueueAction(
                provider,
                (p, a, c) => p.WriteItemObject(a.item, a.path, a.isContainer),
                (item, path, isContainer),
                cancellationToken);

            return task;
        }

        public static Task WriteVerboseAsync<TProvider>(
            this TProvider provider,
            string text,
            CancellationToken cancellationToken = default)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            var context = AsyncMethodContext.GetContext(provider);

            var task = context.QueueAction(
                provider,
                (p, a, c) => p.WriteVerbose(a),
                text,
                cancellationToken);

            return task;
        }
    }
}
