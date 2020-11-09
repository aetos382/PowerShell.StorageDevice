using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsync
{
    public interface IAsyncItemCmdletProvider :
        IAsyncDriveCmdletProvider
    {
        Task ClearItemAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<object?> ClearItemDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        IAsyncEnumerable<string> ExpandPathAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task GetItemAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<object> GetItemDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task InvokeDefaultActionAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<object> InvokeDefaultActionDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<bool> IsValidPathAsync(
            string path);

        Task<bool> ItemExistsAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task<object> ItemExistsDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        Task SetItemAsync(
            string path,
            object value)
        {
            throw new PSNotImplementedException();
        }

        Task<object> SetItemDynamicParametersAsync(
            string path,
            object value)
        {
            throw new PSNotImplementedException();
        }
    }
}
