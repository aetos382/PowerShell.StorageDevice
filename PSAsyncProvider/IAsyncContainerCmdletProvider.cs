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
            throw new PSNotImplementedException();
        }

        ValueTask CopyItemAsync(
            string path,
            string copyPath,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object?> CopyItemDynamicParametersAsync(
            string path,
            string copyPath,
            bool recurse)
        {
            throw new PSNotImplementedException();
        }

        ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> GetChildItemsDynamicParametersAsync(
            string path,
            bool recurse)
        {
            throw new PSNotImplementedException();
        }

        ValueTask GetChildNamesAsync(
            string path,
            ReturnContainers returnContainers,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> GetChildNamesDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<bool> HasChildItemsAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask NewItemAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> NewItemDynamicParametersAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotImplementedException();
        }

        ValueTask RemoveItemAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask RemoveItemDynamicParametersAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask RenameItemAsync(
            string path,
            string newName)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> RenameItemDynamicParametersAsync(
            string path,
            string newName)
        {
            throw new PSNotImplementedException();
        }
    }
}
