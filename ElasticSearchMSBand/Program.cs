namespace ElasticSearchMSBand
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;

    class Program
    {
        private static RegistryManager registryManager = RegistryManager.CreateFromConnectionString(Constants.ConnectionString);
        private static List<SimDevice> devices = new List<SimDevice>();
        private static Random rand = new Random();
        private static object listLock = new Object();

        static void Main(string[] args)
        {
            Console.WriteLine($"Using ConnectionString: {Constants.ConnectionString}");

            // Bypass https cert validation so we can use fiddler
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            ServicePointManager.Expect100Continue = false;

            CancellationTokenSource cts = new CancellationTokenSource();
            try
            {
                RegisterAllDevicesAsync(cts.Token).Wait(cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception adding Devices: {e}");
            }

            try
            {
                var simulations = SimulateDevices(cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception simulating Devices: {e}");
            }

            Console.WriteLine("Hit Enter to Exit!!");
            Console.ReadLine();
            Console.WriteLine("##### Stopping simulation (wait 1 second)");
            cts.Cancel();
            Thread.Sleep(1000);
            
            cts.Dispose();
            Console.WriteLine("Exiting");
        }

        private static async Task SimulateDevices(CancellationToken token)
        {
            Console.WriteLine($"##### Simulate {devices.Count} devices");

            var simulations = new List<Task>(devices.Count);
            simulations.AddRange(devices.Select(simDevice => SimulateSingleDevice(simDevice, token)));

            await Task.WhenAll(simulations);
        }

        private static async Task SimulateSingleDevice(SimDevice simDevice, CancellationToken token)
        {
            // random start so they don't all start at once
            await Task.Delay(rand.Next(Convert.ToInt32(Constants.simulationDelay.TotalMilliseconds))/5, token);

            DeviceClient client = DeviceClient.CreateFromConnectionString(Constants.ConnectionString, simDevice.device.Id);
            var deviceData = simDevice.CurrentData;

            while (!token.IsCancellationRequested)
            {
                bool flip = (rand.Next(2) == 0);

                NewRandomVal(ref deviceData.maxElevation, 50, 5000, 50, 5);
                deviceData.minElevation = deviceData.maxElevation - rand.Next(50);
                NewRandomVal(ref deviceData.actualDistance, 0, 3, 2, 1000);
                deviceData.stepsTaken = deviceData.actualDistance * 250;
                deviceData.totalDistance = deviceData.actualDistance;
                if (flip)
                {
                    deviceData.longitude = deviceData.longitude - (deviceData.actualDistance * 0.022);
                }
                else
                {
                    deviceData.longitude = deviceData.longitude + (deviceData.actualDistance * 0.022);
                }

                NewRandomVal(ref deviceData.averageHeartRate, 60, 180, 7, 5);
                deviceData.lowestHeartRate = deviceData.averageHeartRate - rand.Next(7);
                deviceData.peakHeartRate = deviceData.averageHeartRate + rand.Next(7);
                NewRandomVal(ref deviceData.totalCalories, 2, 55, 15, 2);
                NewRandomVal(ref deviceData.speed, 0, 23, 4, 10);
                deviceData.endTime = DateTime.UtcNow;
                deviceData.startTime = deviceData.endTime - Constants.simulationDelay;

                var messageString = JsonConvert.SerializeObject(deviceData);
                var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(messageString));

                Console.WriteLine($"Simulate device: {simDevice.device.Id} : {messageString}");
                await client.SendEventAsync(message);

                await Task.Delay(Constants.simulationDelay, token);
            }

            Console.WriteLine($"Simulate device cancelled: {simDevice.device.Id}");
        }

        public static int NewRandomVal(ref int currentVal, int minVal, int maxVal, int variance, int flipFreq)
        {
            bool flip = (rand.Next(flipFreq) == 0);

            int newVal = rand.Next(
                Math.Max((currentVal - variance), minVal),
                Math.Min((currentVal + variance), maxVal));
            
            currentVal = newVal;
            
            return newVal;
        }

        private static async Task RegisterAllDevicesAsync(CancellationToken token)
        {
            Console.WriteLine($"##### Register {Constants.numDevices} devices");

            var tasks = new List<Task>();
            var devicesToAdd = Constants.numDevices;
            var deviceNum = 0;

            while (devicesToAdd > 0)
            {
                devicesToAdd--;
                deviceNum++;
                tasks.Add(AddDeviceAsync($"MSBand-{deviceNum}", token));
                if (tasks.Count == 20)
                {
                    Console.WriteLine($"batch of 20 tasks, {devicesToAdd} left");
                    await Task.WhenAll(tasks);
                    Console.WriteLine($"batch done");
                    tasks.Clear();
                }
            }

            await Task.WhenAll(tasks);
        }

        private static async Task AddDeviceAsync(string deviceId, CancellationToken token)
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
    }
}
