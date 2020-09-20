using System.Management.Automation.Provider;

namespace PSAsyncProvider
{
    public abstract class AsyncDriveCmdletProvider :
        DriveCmdletProvider,
        IAsyncDriveCmdletProvider
    {
    }
}
