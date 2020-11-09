using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync.Provider
{
    public interface IAsyncContentWriter
    {
        Task CloseAsync();

        IAsyncEnumerable<T> WriteAsync<T>(
            IAsyncEnumerable<T> content,
            CancellationToken cancellationToken);

        Task SeekAsync(
            long offset,
            SeekOrigin origin,
            CancellationToken cancellationToken);
    }
}
