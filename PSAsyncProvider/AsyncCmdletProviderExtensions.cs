using System;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public static class AsyncCmdletProviderExtensions
    {
        public static Task WriteItemObjectAsync(
            object item,
            string path,
            bool isContainer,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
