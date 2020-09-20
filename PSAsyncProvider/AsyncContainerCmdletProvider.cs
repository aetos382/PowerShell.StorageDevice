using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public abstract class AsyncContainerCmdletProvider :
        ContainerCmdletProvider,
        IAsyncContainerCmdletProvider
    {
        public abstract ValueTask<bool> IsValidPathAsync(
            string path);
    }
}
