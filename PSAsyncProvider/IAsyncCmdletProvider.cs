using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncCmdletProvider
    {
        ValueTask<ProviderInfo> StartAsync(
            ProviderInfo providerInfo)
        {
            throw new PSNotImplementedException();
        }

        ValueTask<object> StartDynamicParametersAsync()
        {
            throw new PSNotImplementedException();
        }

        ValueTask StopAsync()
        {
            throw new PSNotImplementedException();
        }
    }
}
