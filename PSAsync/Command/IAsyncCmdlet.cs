using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync.Command
{
    public interface IAsyncCmdlet :
        ICmdlet
    {
        Task BeginProcessingAsync(
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task ProcessRecordAsync(
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task EndProcessingAsync(
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }
    }
}
