using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Windows.Devices.Enumeration;
using Windows.Devices.Portable;

namespace PowerShellStorageDevice
{
    internal class StorageDeviceManager
    {
        public static async IAsyncEnumerable<DeviceInformation> GetStorageDevices()
        {
            var selector = StorageDevice.GetDeviceSelector();

            var devices = await DeviceInformation.FindAllAsync(selector);

            foreach (var device in devices)
            {
                yield return device;
            }
        }

        public static async ValueTask<DeviceInformation> GetStorageDevice(
            string deviceId)
        {
            var device = await DeviceInformation.CreateFromIdAsync(deviceId);
            return device;
        }
    }
}
