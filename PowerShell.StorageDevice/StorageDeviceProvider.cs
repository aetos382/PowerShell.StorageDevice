using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading.Tasks;

using Microsoft;

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

        public Task<bool> IsValidPathAsync(
            string path)
        {
            return Task.FromResult(true);
        }

        protected override ProviderInfo Start(
            ProviderInfo providerInfo)
        {
            var result = this.ExecuteAsyncAction(
                (p, s, c) => p.StartAsync(s),
                providerInfo);

            return result;
        }

        public async Task<ProviderInfo> StartAsync(
            ProviderInfo providerInfo)
        {
            await this.WriteVerboseAsync("StartAsync");

            await Task.Yield();

            await this.WriteVerboseAsync("StartAsync2");

            return providerInfo;
        }

        public async Task<bool> ItemExistsAsync(
            string path)
        {
            Requires.NotNull(path, nameof(path));

            if (path.Length == 0)
            {
                return true;
            }

            return false;
        }

        protected override bool ConvertPath(string path, string filter, ref string updatedPath, ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        protected override string[] ExpandPath(string path)
        {
            return base.ExpandPath(path);
        }

        protected override void GetItem(string path)
        {
            base.GetItem(path);
        }

        protected override string GetParentPath(string path, string root)
        {
            return base.GetParentPath(path, root);
        }

        protected override string MakePath(string parent, string child)
        {
            return base.MakePath(parent, child);
        }

        public Task<bool> IsItemContainerAsync(
            string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        /*
        public async Task GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }
        }

        public async Task GetChildItemsAsync(
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
        */
    }
}
