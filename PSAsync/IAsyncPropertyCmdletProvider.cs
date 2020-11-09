using System.Collections.Generic;
using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

namespace PSAsync
{
    public interface IAsyncPropertyCmdletProvider
    {
        Task ClearPropertyAsync(
            string path,
            IReadOnlyCollection<string> propertyToClear,
            CancellationToken cancellationToken);

        Task<object> ClearPropertyDynamicParameterAsync(
            string path,
            IReadOnlyCollection<string> propertyToClear,
            CancellationToken cancellationToken);

        Task GetPropertyAsync(
            string path,
            IReadOnlyCollection<string> providerSpecificPickList,
            CancellationToken cancellationToken);

        Task<object> GetPropertyDynamicParameterAsync(
            string path,
            IReadOnlyCollection<string> providerSpecificPickList,
            CancellationToken cancellationToken);

        Task SetPropertyAsync(
            string path,
            PSObject propertyValue,
            CancellationToken cancellationToken);

        Task<object> SetPropertyDynamicParameterAsync(
            string path,
            PSObject propertyValue,
            CancellationToken cancellationToken);
    }
}
