using System.Threading;
using System.Threading.Tasks;

namespace PSAsync.Provider
{
    public interface IAsyncContentCmdletProvider
    {
        Task ClearContentAsync(
            CancellationToken cancellationToken);

        Task<object> ClearContentDynamicParametersAsync(
            string path,
            CancellationToken cancellationToken);

        Task<IAsyncContentReader> GetContentReaderAsync(
            string path,
            CancellationToken cancellationToken);

        Task<object> GetContentReaderDynamicParametersAsync(
            string path,
            CancellationToken cancellationToken);

        Task<IAsyncContentWriter> GetContentWriterAsync(
            string path,
            CancellationToken cancellationToken);

        Task<object> GetContentWriterDynamicParameters(
            string path,
            CancellationToken cancellationToken);
    }
}
