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

        protected override ProviderInfo Start(
            ProviderInfo providerInfo)
        {
            this.WriteVerbose(nameof(this.Start));

            return base.Start(providerInfo);
        }

        protected override void Stop()
        {
            this.WriteVerbose(nameof(this.Stop));

            base.Stop();
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
