using System.Collections.Generic;

using Windows.Devices.Enumeration;
using Windows.Devices.Portable;

namespace PowerShellStorageDevice
{
    internal class StorageDeviceManager
    {
        public static IEnumerable<DeviceInformation> GetDevices()
        {
            var deviceSelector = StorageDevice.GetDeviceSelector();
            var devices = DeviceInformation.FindAllAsync(deviceSelector).GetResults();

            foreach (var device in devices)
            {
                yield return device;
            }
        }
    }
}
