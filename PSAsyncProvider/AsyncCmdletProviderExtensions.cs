using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public static class AsyncCmdletProviderExtensions
    {
        public static Task WriteItemObjectAsync(
            this IAsyncCmdletProvider provider,
            object item,
            string path,
            bool isContainer,
            CancellationToken cancellationToken)
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            throw new NotImplementedException();
        }
    }
}
