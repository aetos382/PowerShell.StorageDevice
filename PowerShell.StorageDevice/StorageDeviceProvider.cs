using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using PSAsyncProvider;

namespace PowerShellStorageDevice
{
    [CmdletProvider(ProviderName, SupportedCapabilities)]
    [GenerateMember]
    public partial class StorageDeviceProvider :
        NavigationCmdletProvider,
        IAsyncNavigationCmdletProvider
    {
        private const string ProviderName = "StorageDevice";

        private const ProviderCapabilities SupportedCapabilities =
            ProviderCapabilities.Filter |
            ProviderCapabilities.Include |
            ProviderCapabilities.Exclude |
            ProviderCapabilities.ExpandWildcards |
            ProviderCapabilities.ShouldProcess;

        public StorageDeviceProvider()
        {
        }

        public ValueTask<bool> IsValidPathAsync(
            string path)
        {
            return new ValueTask<bool>(true);
        }

        public ValueTask<bool> ItemExistsAsync(
            string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }

        public ValueTask<bool> IsItemContainerAsync(
            string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                return new ValueTask<bool>(true);
            }

            return new ValueTask<bool>(false);
        }

        public async ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
        }

        public async ValueTask GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
        }
    }
}
