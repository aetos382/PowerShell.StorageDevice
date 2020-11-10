using System.Management.Automation;
using System.Threading;
using System.Threading.Tasks;

using Windows.Devices.Enumeration;

using PSAsync;
using PSAsync.Command;

namespace PowerShellStorageDevice.Command
{
    [Cmdlet(VerbsCommon.Get, "StorageDevice")]
    [OutputType(typeof(DeviceInformation))]
    public class GetStorageDeviceCommand :
        AsyncCmdlet
    {
        public override async Task ProcessRecordAsync(
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
