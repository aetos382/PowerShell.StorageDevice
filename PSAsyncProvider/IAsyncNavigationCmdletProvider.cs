using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncNavigationCmdletProvider :
        IAsyncContainerCmdletProvider
    {
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
