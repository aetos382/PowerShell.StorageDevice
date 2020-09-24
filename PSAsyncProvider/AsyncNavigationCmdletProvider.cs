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

        protected override bool IsValidPath(
            string path)
        {
            return this.IsValidPathAsync(path).Result;
        }

        protected override bool IsItemContainer(
            string path)
        {
            return ((IAsyncNavigationCmdletProvider) this).IsItemContainerAsync(path).Result;
        }
    }
}
