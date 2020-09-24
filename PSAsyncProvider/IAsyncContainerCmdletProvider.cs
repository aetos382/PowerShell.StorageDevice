using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncContainerCmdletProvider :
        IAsyncItemCmdletProvider
    {
        public virtual ValueTask<PathConversionResult> ConvertPathAsync(
            string path,
            string filter)
        {
            return new ValueTask<PathConversionResult>(
                PathConversionResult.NotAltered(path, filter));
        }

        public virtual ValueTask CopyItemAsync(
            string path,
            string copyPath,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object?> CopyItemDynamicParametersAsync(
            string path,
            string copyPath,
            bool recurse)
        {
            return default;
        }

        public virtual ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> GetChildItemsDynamicParametersAsync(
            string path,
            bool recurse)
        {
            return default;
        }

        public virtual ValueTask GetChildNamesAsync(
            string path,
            ReturnContainers returnContainers,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> GetChildNamesDynamicParametersAsync(
            string path)
        {
            return default;
        }

        public virtual ValueTask<bool> HasChildItemsAsync(
            string path)
        {
            return default;
        }

        public virtual ValueTask NewItemAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> NewItemDynamicParametersAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            return default;
        }

        public virtual ValueTask RemoveItemAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask RemoveItemDynamicParametersAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            return default;
        }

        public virtual ValueTask RenameItemAsync(
            string path,
            string newName)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> RenameItemDynamicParametersAsync(
            string path,
            string newName)
        {
            return default;
        }
    }
}
