using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public abstract class AsyncNavigationCmdletProvider :
        NavigationCmdletProvider,
        IAsyncNavigationCmdletProvider
    {
        public abstract ValueTask<bool> IsValidPathAsync(
            string path);
    }
}
