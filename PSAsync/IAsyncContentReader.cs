using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync
{
    public interface IAsyncContentReader :
        IDisposable,
        IAsyncDisposable
    {
        Task CloseAsync();

        IAsyncEnumerable<T> ReadAsync<T>(
            long readCount,
            CancellationToken cancellationToken);

        Task SeekAsync(
            long offset,
            SeekOrigin origin,
            CancellationToken cancellationToken);
    }
}
