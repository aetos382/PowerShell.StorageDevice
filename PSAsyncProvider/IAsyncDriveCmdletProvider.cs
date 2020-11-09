using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncDriveCmdletProvider :
        IAsyncCmdletProvider
    {
        IAsyncEnumerable<PSDriveInfo> InitializeDefaultDrivesAsync()
        {
            throw new PSNotImplementedException();
        }

        ValueTask<PSDriveInfo> NewDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object?> NewDriveDynamicParametersAsync()
        {
            throw new PSNotImplementedException();
        }

        ValueTask<PSDriveInfo> RemoveDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotImplementedException();
        }
    }
}
