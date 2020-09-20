using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public abstract class AsyncItemCmdletProvider :
        ItemCmdletProvider,
        IAsyncItemCmdletProvider
    {
        public abstract ValueTask<bool> IsValidPathAsync(
            string path);
    }
}
