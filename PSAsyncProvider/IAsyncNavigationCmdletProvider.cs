using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncNavigationCmdletProvider :
        IAsyncContainerCmdletProvider
    {
        Task<string> GetChildNameAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<string> GetParentPathAsync(
            string path,
            string root)
        {
            throw new PSNotImplementedException();
        }

        Task<bool> IsItemContainerAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<string> MakePathAsync(
            string parent,
            string child)
        {
            throw new PSNotImplementedException();
        }

        Task<string> MakePathAsync(
            string parent,
            string child,
            bool childIsLeaf)
        {
            throw new PSNotImplementedException();
        }

        Task MoveItemAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task<object> MoveItemDynamicParametersAsync(
            string path,
            string destination,
            CancellationToken cancellationToken)
        {
            throw new PSNotImplementedException();
        }

        Task<string> NormalizeRelativePathAsync(
            string path,
            string basePath)
        {
            throw new PSNotImplementedException();
        }
    }
}
