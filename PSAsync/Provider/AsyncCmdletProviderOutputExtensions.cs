using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

namespace PSAsync.Provider
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
                (p, a, c) => p.WriteItemObject(a.item, a.path, a.isContainer),
                (item, path, isContainer),
                cancellationToken);

            return task;
        }
    }
}
