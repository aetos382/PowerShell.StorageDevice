using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncCmdletProvider
    {
        ValueTask<ProviderInfo> StartAsync(
            ProviderInfo providerInfo)
        {
            return new ValueTask<ProviderInfo>(providerInfo);
        }

        ValueTask<object> StartDynamicParametersAsync()
        {
            return default;
        }

        ValueTask StopAsync()
        {
            return default;
        }
    }
}
