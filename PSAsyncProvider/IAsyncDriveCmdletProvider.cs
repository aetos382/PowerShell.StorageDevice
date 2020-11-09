using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncDriveCmdletProvider :
        IAsyncCmdletProvider
    {
        async IAsyncEnumerable<PSDriveInfo> InitializeDefaultDrivesAsync()
        {
            yield break;
        }

        ValueTask<PSDriveInfo> NewDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object?> NewDriveDynamicParametersAsync()
        {
            return default;
        }

        ValueTask<PSDriveInfo> RemoveDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotSupportedException();
        }
    }
}
