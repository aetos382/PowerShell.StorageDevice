using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using PSAsyncProvider;

namespace PowerShellStorageDevice
{
    [CmdletProvider(ProviderName, SupportedCapabilities)]
    public class StorageDeviceProvider :
        AsyncNavigationCmdletProvider
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
            return base.Start(providerInfo);
        }

        protected override void Stop()
        {
            base.Stop();
        }

        public override ValueTask<bool> IsValidPathAsync(
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

            if (path.Length == 0)
            {
                var devices = StorageDeviceManager.GetDevices();

                foreach (var device in devices)
                {
                    await this
                        .WriteItemObjectAsync(device, device.Name, true, cancellationToken)
                        .ConfigureAwait(false);
                }

                return;
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

            if (path.Length == 0)
            {
                var devices = StorageDeviceManager.GetDevices();

                foreach (var device in devices)
                {
                    await this
                        .WriteItemObjectAsync(device, device.Name, true, cancellationToken)
                        .ConfigureAwait(false);
                }

                return;
            }
        }
    }
}
