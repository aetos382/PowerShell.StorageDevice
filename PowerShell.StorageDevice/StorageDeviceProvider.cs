using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PowerShellStorageDevice
{
    [CmdletProvider(ProviderName, SupportedCapabilities)]
    public class StorageDeviceProvider :
        NavigationCmdletProvider,
        IContentCmdletProvider,
        IPropertyCmdletProvider,
        IDisposable,
        IAsyncDisposable
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

        protected override bool IsValidPath(
            string path)
        {
            return true;
        }

        protected override void GetItem(
            string path)
        {
            base.GetItem(path);
        }

        protected override bool ItemExists(
            string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                return true;
            }

            return false;
        }

        protected override bool ConvertPath(
            string path,
            string filter,
            ref string updatedPath,
            ref string updatedFilter)
        {
            return base.ConvertPath(path, filter, ref updatedPath, ref updatedFilter);
        }

        protected override string[] ExpandPath(
            string path)
        {
            return base.ExpandPath(path);
        }

        protected override bool IsItemContainer(
            string path)
        {
            if (path is null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Length == 0)
            {
                return true;
            }

            return false;
        }

        protected override bool HasChildItems(
            string path)
        {
            return base.HasChildItems(path);
        }

        protected override string GetChildName(
            string path)
        {
            return base.GetChildName(path);
        }

        protected override void GetChildNames(
            string path,
            ReturnContainers returnContainers)
        {
            base.GetChildNames(path, returnContainers);
        }
        
        protected override void GetChildItems(
            string path,
            bool recurse)
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
                    this.WriteItemObject(device, device.Name, true);
                }

                return;
            }
            
            base.GetChildItems(path, recurse);
        }

        protected override void GetChildItems(
            string path,
            bool recurse,
            uint depth)
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
                    this.WriteItemObject(device, device.Name, true);
                }

                return;
            }

            base.GetChildItems(path, recurse, depth);
        }

        public void ClearProperty(
            string path,
            Collection<string> propertyToClear)
        {
            throw new NotImplementedException();
        }

        public object ClearPropertyDynamicParameters(
            string path,
            Collection<string> propertyToClear)
        {
            return null;
        }

        public void GetProperty(
            string path,
            Collection<string> providerSpecificPickList)
        {
            throw new NotImplementedException();
        }

        public object GetPropertyDynamicParameters(
            string path,
            Collection<string> providerSpecificPickList)
        {
            return null;
        }

        public void SetProperty(
            string path,
            PSObject propertyValue)
        {
            throw new NotImplementedException();
        }

        public object SetPropertyDynamicParameters(
            string path,
            PSObject propertyValue)
        {
            return null;
        }

        public void Dispose()
        {
        }

        public ValueTask DisposeAsync()
        {
            return default;
        }

        public void ClearContent(
            string path)
        {
            throw new NotImplementedException();
        }

        public object ClearContentDynamicParameters(
            string path)
        {
            return null;
        }

        public IContentReader GetContentReader(
            string path)
        {
            throw new NotImplementedException();
        }

        public object GetContentReaderDynamicParameters(
            string path)
        {
            return null;
        }

        public IContentWriter GetContentWriter(
            string path)
        {
            throw new NotImplementedException();
        }

        public object GetContentWriterDynamicParameters(
            string path)
        {
            return null;
        }
    }
}
