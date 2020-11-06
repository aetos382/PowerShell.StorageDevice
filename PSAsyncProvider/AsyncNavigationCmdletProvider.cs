using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public abstract class AsyncNavigationCmdletProvider :
        NavigationCmdletProvider
    {
        public abstract ValueTask<bool> IsValidPathAsync(
            string path);

        protected override bool IsValidPath(
            string path)
        {
            this.WriteVerbose(nameof(this.IsValidPath));

            return this.IsValidPathAsync(path).Result;
        }

        protected override bool IsItemContainer(
            string path)
        {
            this.WriteVerbose(nameof(this.IsItemContainer));

            return ((IAsyncNavigationCmdletProvider) this).IsItemContainerAsync(path).Result;
        }

        protected override bool ItemExists(string path)
        {
            this.WriteVerbose(nameof(this.ItemExists));

            return this.ItemExistsAsync(path).Result;
        }

        public abstract ValueTask<bool> ItemExistsAsync(
            string path);

        public virtual ValueTask<string> GetChildNameAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask<string> GetParentPathAsync(
            string path,
            string root)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask<bool> IsItemContainerAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask<string> MakePathAsync(
            string parent,
            string child)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<string> MakePathAsync(
            string parent,
            string child,
            bool childIsLeaf)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask MoveItemAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> MoveItemDynamicParametersAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            return default;
        }

        public virtual ValueTask<string> NormalizeRelativePathAsync(
            string path,
            string basePath)
        {
            throw new PSNotImplementedException();
        }
    }
}
