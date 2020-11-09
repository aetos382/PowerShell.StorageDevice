using System.Threading;
using System.Threading.Tasks;

namespace PSAsync.Command
{
    public interface IAsyncCmdlet :
        ICmdletOutput
    {
        Task BeginProcessingAsync(
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        Task ProcessRecordAsync(
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        Task EndProcessingAsync(
            CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
