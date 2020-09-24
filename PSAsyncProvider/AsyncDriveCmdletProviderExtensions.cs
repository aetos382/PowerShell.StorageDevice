using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Provider;
using System.Threading.Tasks;

namespace PSAsyncProvider
{
    public static class AsyncDriveCmdletProviderExtensions
    {
        public static Collection<PSDriveInfo> InvokeInitializeDefaultDrivesAsync<TProvider>(
            this TProvider provider)
            where TProvider :
                DriveCmdletProvider,
                IAsyncDriveCmdletProvider
        {
            if (provider is null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            return Hoge().Result;

            async ValueTask<Collection<PSDriveInfo>> Hoge()
            {
                var results = new Collection<PSDriveInfo>();

                var drives = provider.InitializeDefaultDrivesAsync();

                await foreach (var drive in drives)
                {
                    results.Add(drive);
                }

                return results;
            }
        }
    }
}
