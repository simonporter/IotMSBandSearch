using System.Linq;

namespace ElasticSearchMSBand
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices;

    public static class DeviceRegistrar
    {
        private static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Constants.ConnectionString);
        private static object listLock = new Object();

        public static async Task<List<SimDevice>> RegisterDevicesAsync(int numDevices, CancellationToken token)
        {
            Console.WriteLine($"##### Register {numDevices} devices");

            List<SimDevice> devices = new List<SimDevice>();

            var tasks = new List<Task>();
            var devicesToAdd = numDevices;
            var deviceNum = Constants.DeviceSeed;

            while (devicesToAdd > 0)
            {
                devicesToAdd--;
                deviceNum++;
                tasks.Add(AddDeviceAsync(devices, $"MSBand-{deviceNum}", token));
                if (tasks.Count == 20)
                {
                    Console.WriteLine($"batch of 20 tasks, {devicesToAdd} left");
                    await Task.WhenAll(tasks);
                    Console.WriteLine($"batch done");
                    tasks.Clear();
                }
            }

            await Task.WhenAll(tasks);
            return devices;
        }

        private static async Task AddDeviceAsync(List<SimDevice> devices, string deviceId, CancellationToken token)
        {
            Device device = await registryManager.GetDeviceAsync(deviceId, token) ??
                            await registryManager.AddDeviceAsync(new Device(deviceId), token);

            lock (listLock)
            {
                devices.Add(new SimDevice(device));
            }

            Console.WriteLine($"Generated device key [{device.Id}]: {device.Authentication.SymmetricKey.PrimaryKey}");
            await Task.Delay(1, token);
        }

        public static async Task<List<SimDevice>> GetExistingDevices(int numDevices, CancellationToken token)
        {
            Console.WriteLine($"##### Get {numDevices} existing devices");

            List<SimDevice> devices = new List<SimDevice>();

            var existingDevices = await registryManager.GetDevicesAsync(numDevices, token);

            foreach (var existingDevice in existingDevices)
            {
                lock (listLock)
                {
                    devices.Add(new SimDevice(existingDevice));
                }

                Console.WriteLine($"Existing device [{existingDevice.Id}]: {existingDevice.Authentication.SymmetricKey.PrimaryKey}");
            }

            return devices;
        }
    }
}
