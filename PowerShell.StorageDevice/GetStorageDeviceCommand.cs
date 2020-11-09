using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Windows.Devices.Enumeration;

using PSAsync;
using PSAsync.Command;

namespace PowerShellStorageDevice
{
    [Cmdlet(VerbsCommon.Get, "StorageDevice")]
    [OutputType(typeof(DeviceInformation))]
    public class GetStorageDeviceCommand :
        Cmdlet,
        IAsyncCmdlet
    {
        protected override void ProcessRecord()
        {
            this.ExecuteAsyncAction((c, t) => c.ProcessRecordAsync(t));
        }

        public async Task ProcessRecordAsync(
            CancellationToken cancellationToken)
        {
            var devices = StorageDeviceManager.GetStorageDevices();

            await foreach (var device in devices)
            {
                await this
                    .WriteObjectAsync(device, cancellationToken)
                    .ConfigureAwait(false);
            }
        }
    }
}
