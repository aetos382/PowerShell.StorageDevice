using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncContainerCmdletProvider :
        IAsyncItemCmdletProvider
    {
        Task<PathConversionResult> ConvertPathAsync(
            string path,
            string filter)
        {
            throw new PSNotImplementedException();
        }

        Task CopyItemAsync(
            string path,
            string copyPath,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task<object?> CopyItemDynamicParametersAsync(
            string path,
            string copyPath,
            bool recurse)
        {
            throw new PSNotImplementedException();
        }

        Task GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task<object> GetChildItemsDynamicParametersAsync(
            string path,
            bool recurse)
        {
            throw new PSNotImplementedException();
        }

        Task GetChildNamesAsync(
            string path,
            ReturnContainers returnContainers,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task<object> GetChildNamesDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<bool> HasChildItemsAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task NewItemAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotImplementedException();
        }

        Task<object> NewItemDynamicParametersAsync(
            string path,
            string itemTypeName,
            object newItemValue)
        {
            throw new PSNotImplementedException();
        }

        Task RemoveItemAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task RemoveItemDynamicParametersAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task RenameItemAsync(
            string path,
            string newName)
        {
            throw new PSNotImplementedException();
        }

        Task<object> RenameItemDynamicParametersAsync(
            string path,
            string newName)
        {
            throw new PSNotImplementedException();
        }
    }
}
