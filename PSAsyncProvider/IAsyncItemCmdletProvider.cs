using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncItemCmdletProvider :
        IAsyncDriveCmdletProvider
    {
        public virtual ValueTask ClearItemAsync(
            string path)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object?> ClearItemDynamicParametersAsync(
            string path)
        {
            return default;
        }

        public virtual IAsyncEnumerable<string> ExpandPathAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask GetItemAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask<object> GetItemDynamicParametersAsync(
            string path)
        {
            return default;
        }

        public virtual ValueTask InvokeDefaultActionAsync(
            string path)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> InvokeDefaultActionDynamicParametersAsync(
            string path)
        {
            return default;
        }

        public abstract ValueTask<bool> IsValidPathAsync(
            string path);

        public virtual ValueTask<bool> ItemExistsAsync(
            string path)
        {
            throw new PSNotImplementedException();
        }

        public virtual ValueTask<object> ItemExistsDynamicParametersAsync(
            string path)
        {
            return default;
        }

        public virtual ValueTask SetItemAsync(
            string path,
            object value)
        {
            throw new PSNotSupportedException();
        }

        public virtual ValueTask<object> SetItemDynamicParametersAsync(
            string path,
            object value)
        {
            return default;
        }
    }
}
