using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading;
using System.Threading.Tasks;

using Microsoft;

using PSAsync;
using PSAsync.Provider;

namespace PowerShellStorageDevice.Provider
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

        protected override Collection<PSDriveInfo> InitializeDefaultDrives()
        {
            var drives = this.ExecuteAsyncMethod(
                async (p, c) => {
                    var drives = await this
                        .InitializeDefaultDrivesAsync()
                        .ToListAsync(c)
                        .ConfigureAwait(false);

                    return new Collection<PSDriveInfo>(drives);
                });

            return drives!;
        }

        protected override PSDriveInfo NewDrive(PSDriveInfo drive)
        {
            return this.ExecuteAsyncMethod(
                (p, a, _) => this.NewDriveAsync(a),
                drive);
        }

        protected override PSDriveInfo RemoveDrive(PSDriveInfo drive)
        {
            return base.RemoveDrive(drive);
        }

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
            this.ExecuteAsyncMethod(
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

            this.ExecuteAsyncMethod(
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

            this.ExecuteAsyncMethod(
                (p, a, c) => this.GetChildItemsAsync(a.path, a.recurse, a.depth, c),
                (path, recurse, depth));
        }
        
        protected override void GetChildNames(
            string path,
            ReturnContainers returnContainers)
        {
            Requires.NotNull(path, nameof(path));

            this.WriteVerbose("GetChildNames");

            this.ExecuteAsyncMethod(
                (p, a, c) => p.GetChildNamesAsync(a.path, a.returnContainers, c),
                (path, returnContainers));
        }

        #endregion

        public async IAsyncEnumerable<PSDriveInfo> InitializeDefaultDrivesAsync()
        {
            yield break;
        }

        public async Task<PSDriveInfo> NewDriveAsync(
            PSDriveInfo drive)
        {
            try
            {
                var device = await StorageDeviceManager
                    .GetStorageDevice(drive.Root)
                    .ConfigureAwait(false);

                return drive;
            }
            catch (FileNotFoundException ex)
            {
                var error = new ErrorRecord(
                    new ItemNotFoundException(ex.Message, ex),
                    "DeviceNotFound",
                    ErrorCategory.ObjectNotFound,
                    null);

                this.ThrowTerminatingError(error);

                // ここには来ない
                throw;
            }
        }

        public Task<PSDriveInfo> RemoveDriveAsync(
            PSDriveInfo drive)
        {
            throw new PSNotImplementedException();
        }

        public Task<bool> IsValidPathAsync(
            string path)
        {
            return Task.FromResult(true);
        }

        protected override ProviderInfo Start(
            ProviderInfo providerInfo)
        {
            var result = this.ExecuteAsyncMethod(
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
