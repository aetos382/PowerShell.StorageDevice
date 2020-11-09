using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsync.Provider
{
    public interface IAsyncDriveCmdletProvider :
        IAsyncCmdletProvider
    {
        IAsyncEnumerable<PSDriveInfo> InitializeDefaultDrivesAsync()
        {
            throw new PSNotImplementedException();
        }

        Task<PSDriveInfo> NewDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotImplementedException();
        }

        Task<object?> NewDriveDynamicParametersAsync()
        {
            throw new PSNotImplementedException();
        }

        Task<PSDriveInfo> RemoveDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotImplementedException();
        }
    }
}
