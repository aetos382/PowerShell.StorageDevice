using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncDriveCmdletProvider :
        IAsyncCmdletProvider
    {
        public virtual async IAsyncEnumerable<PSDriveInfo> InitializeDefaultDrivesAsync()
        {
            yield break;
        }

        public virtual ValueTask<PSDriveInfo> NewDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object?> NewDriveDynamicParametersAsync()
        {
            return default;
        }

        public virtual ValueTask<PSDriveInfo> RemoveDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotSupportedException();
        }
    }
}
