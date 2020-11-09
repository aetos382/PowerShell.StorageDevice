using System;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

using PSAsync;
using PSAsync.Provider;

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

        #region sync methods

        protected override string GetChildName(
            string path)
        {
            this.WriteVerbose("GetChildName");

            return base.GetChildName(path);
        }

        protected override bool HasChildItems(
            string path)
        {
            this.WriteVerbose("HasChildItems");

            return base.HasChildItems(path);
        }

        protected override string NormalizeRelativePath(
            string path,
            string basePath)
        {
            this.WriteVerbose("NormalizeRelativePath");

            return base.NormalizeRelativePath(path, basePath);
        }

        protected override bool ConvertPath(
            string path,
            string filter,
            ref string updatedPath,
            ref string updatedFilter)
        {
            this.WriteVerbose("ConvertPath");

            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        protected override string[] ExpandPath(
            string path)
        {
            this.WriteVerbose("ExpandPath");

            return base.ExpandPath(path);
        }

        protected override void GetItem(
            string path)
        {
            this.ExecuteAsyncAction(
                (p, s, c) => p.GetItemAsync(path),
                path);
        }

        protected override string GetParentPath(
            string path,
            string root)
        {
            this.WriteVerbose("GetParentPath");

            return base.GetParentPath(path, root);
        }

        protected override string MakePath(
            string parent,
            string child)
        {
            this.WriteVerbose("MakePath");

            return base.MakePath(parent, child);
        }

        protected override void GetChildItems(
            string path,
            bool recurse)
        {
            Requires.NotNull(path, nameof(path));

            this.WriteVerbose("GetChildItems w/o depth");

            this.ExecuteAsyncAction(
                (p, a, c) => this.GetChildItemsAsync(a.path, a.recurse, c),
                (path, recurse));
        }
        
        protected override void GetChildItems(
            string path,
            bool recurse,
            uint depth)
        {
            Requires.NotNull(path, nameof(path));

            this.WriteVerbose("GetChildItems w/ depth");

            this.ExecuteAsyncAction(
                (p, a, c) => this.GetChildItemsAsync(a.path, a.recurse, a.depth, c),
                (path, recurse, depth));
        }
        
        protected override void GetChildNames(
            string path,
            ReturnContainers returnContainers)
        {
            Requires.NotNull(path, nameof(path));

            this.WriteVerbose("GetChildNames");

            this.ExecuteAsyncAction(
                (p, a, c) => p.GetChildNamesAsync(a.path, a.returnContainers, c),
                (path, returnContainers));
        }

        #endregion

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

        public async Task GetItemAsync(
            string path)
        {
            Requires.NotNull(path, nameof(path));
        }

        public Task<bool> IsItemContainerAsync(
            string path)
        {
            Requires.NotNull(path, nameof(path));

            if (path.Length == 0)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }

        public async Task GetChildItemsAsync(
            string path,
            bool recurse,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(path, nameof(path));

            if (path.Length == 0)
            {
                var devices = StorageDeviceManager.GetStorageDevices();

                await foreach (var device in devices)
                {
                    await this
                        .WriteItemObjectAsync(
                            device,
                            device.Id,
                            true,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        public async Task GetChildItemsAsync(
            string path,
            bool recurse,
            uint depth,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(path, nameof(path));

            if (path.Length == 0)
            {
                var devices = StorageDeviceManager.GetStorageDevices();

                await foreach (var device in devices)
                {
                    await this
                        .WriteItemObjectAsync(
                            device,
                            device.Id,
                            true,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }

        public async Task GetChildNamesAsync(
            string path,
            ReturnContainers returnContainers,
            CancellationToken cancellationToken)
        {
            Requires.NotNull(path, nameof(path));

            if (path.Length == 0)
            {
                var devices = StorageDeviceManager.GetStorageDevices();

                await foreach (var device in devices)
                {
                    await this
                        .WriteItemObjectAsync(
                            device.Id,
                            device.Id,
                            true,
                            cancellationToken)
                        .ConfigureAwait(false);
                }
            }
        }
    }
}
