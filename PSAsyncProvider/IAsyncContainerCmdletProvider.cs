using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncContainerCmdletProvider :
        IAsyncItemCmdletProvider
    {
        ValueTask<PathConversionResult> ConvertPathAsync(
            string path,
            string filter)
        {
            return new ValueTask<PathConversionResult>(
                PathConversionResult.NotAltered(path, filter));
        }

        ValueTask CopyItemAsync(
            string path,
            string copyPath,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object?> CopyItemDynamicParametersAsync(
            string path,
            string copyPath,
            bool recurse)
        {
            return default;
        }

        ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> GetChildItemsDynamicParametersAsync(
            string path,
            bool recurse)
        {
            return default;
        }

        ValueTask GetChildNamesAsync(
            string path,
            ReturnContainers returnContainers,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> GetChildNamesDynamicParametersAsync(
            string path)
        {
            return default;
        }

        ValueTask<bool> HasChildItemsAsync(
            string path)
        {
            return default;
        }

        ValueTask NewItemAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> NewItemDynamicParametersAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            return default;
        }

        ValueTask RemoveItemAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotSupportedException();
        }

        ValueTask RemoveItemDynamicParametersAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            return default;
        }

        ValueTask RenameItemAsync(
            string path,
            string newName)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> RenameItemDynamicParametersAsync(
            string path,
            string newName)
        {
            return default;
        }
    }
}
