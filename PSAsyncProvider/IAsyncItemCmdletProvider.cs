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
            throw new PSNotSupportedException();
        }

        ValueTask<object?> ClearItemDynamicParametersAsync(
            string path)
        {
            return default;
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
            return default;
        }

        ValueTask InvokeDefaultActionAsync(
            string path)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> InvokeDefaultActionDynamicParametersAsync(
            string path)
        {
            return default;
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
            return default;
        }

        ValueTask SetItemAsync(
            string path,
            object value)
        {
            throw new PSNotSupportedException();
        }

        ValueTask<object> SetItemDynamicParametersAsync(
            string path,
            object value)
        {
            return default;
        }
    }
}
