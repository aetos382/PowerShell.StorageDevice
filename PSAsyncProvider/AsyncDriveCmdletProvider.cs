using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;

namespace PSAsyncProvider
{
    public abstract class AsyncDriveCmdletProvider :
        DriveCmdletProvider,
        IAsyncDriveCmdletProvider
    {
        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            return this.InvokeInitializeDefaultDrivesAsync();
        }
    }
}
