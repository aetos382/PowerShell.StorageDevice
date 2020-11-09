using System.Collections.Generic;
using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncItemCmdletProvider :
        IAsyncDriveCmdletProvider
    {
        ValueTask ClearItemAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object?> ClearItemDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        IAsyncEnumerable<string> ExpandPathAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask GetItemAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> GetItemDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask InvokeDefaultActionAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> InvokeDefaultActionDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<bool> IsValidPathAsync(
            string path);

        ValueTask<bool> ItemExistsAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> ItemExistsDynamicParametersAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        ValueTask SetItemAsync(
            string path,
            object value)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> SetItemDynamicParametersAsync(
            string path,
            object value)
        {
            throw new PSNotImplementedException();
        }
    }
}
