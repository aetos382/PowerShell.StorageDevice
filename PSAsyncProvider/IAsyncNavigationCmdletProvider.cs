using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncNavigationCmdletProvider :
        IAsyncContainerCmdletProvider
    {
        ValueTask<string> GetChildNameAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<string> GetParentPathAsync(
            string path,
            string root)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<bool> IsItemContainerAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<string> MakePathAsync(
            string parent,
            string child)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<string> MakePathAsync(
            string parent,
            string child,
            bool childIsLeaf)
        {
            throw new PSNotImplementedException();
        }

        ValueTask MoveItemAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> MoveItemDynamicParametersAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<string> NormalizeRelativePathAsync(
            string path,
            string basePath)
        {
            throw new PSNotImplementedException();
        }
    }
}
