using System.Management.Automation.Provider;
using System.Security.AccessControl;
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
                (p, a, _) => ((CmdletProvider)p).WriteItemObject(a.item, a.path, a.isContainer),
                (item, path, isContainer),
                cancellationToken);

            return task;
        }

        public static Task WritePropertyObjectAsync<TProvider>(
            this TProvider provider,
            object propertyValue,
            string path,
            CancellationToken cancellationToken)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            var context = AsyncMethodContext.GetContext(provider);

            var task = context.QueueAction(
                (p, a, _) => ((CmdletProvider)p).WritePropertyObject(a.propertyValue, a.path),
                (propertyValue, path),
                cancellationToken);

            return task;
        }

        public static Task WriteSecurityDescriptorObjectAsync<TProvider>(
            this TProvider provider,
            ObjectSecurity securityDescriptor,
            string path,
            CancellationToken cancellationToken)
            where TProvider :
                CmdletProvider,
                IAsyncCmdletProvider
        {
            Requires.NotNull(provider, nameof(provider));

            var context = AsyncMethodContext.GetContext(provider);

            var task = context.QueueAction(
                (p, a, _) => ((CmdletProvider)p).WriteSecurityDescriptorObject(a.securityDescriptor, a.path),
                (securityDescriptor, path),
                cancellationToken);

            return task;
        }
    }
}
