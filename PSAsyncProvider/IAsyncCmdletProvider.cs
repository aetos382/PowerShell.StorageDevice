using System.Management.Automation;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public interface IAsyncCmdletProvider
    {
        Task<ProviderInfo> StartAsync(
            ProviderInfo providerInfo)
        {
            throw new PSNotImplementedException();
        }

        Task<object> StartDynamicParametersAsync()
        {
            throw new PSNotImplementedException();
        }

        Task StopAsync()
        {
            throw new PSNotImplementedException();
        }
    }
}
