namespace ElasticSearchMSBand
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Azure.Devices.Client;
    using Newtonsoft.Json;

    class DeviceSimulator
    {
        private static Random rand = new Random();

        public static async Task SimulateDevices(List<SimDevice> devices, CancellationToken token)
        {
            Console.WriteLine($"##### Simulate {devices.Count} devices");

            var simulations = new List<Task>(devices.Count);
            simulations.AddRange(devices.Select(simDevice => SimulateSingleDevice(simDevice, token)));

            await Task.WhenAll(simulations);
        }

        private static async Task SimulateSingleDevice(SimDevice simDevice, CancellationToken token)
        {
            // random start so they don't all start at once
            await Task.Delay(rand.Next(Convert.ToInt32(Constants.simulationDelay.TotalMilliseconds)) / 5, token);

            DeviceClient client = DeviceClient.CreateFromConnectionString(Constants.ConnectionString,
                simDevice.device.Id);
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

                await SendDeviceEventAsync(simDevice, deviceData, client, token);
            }

            Console.WriteLine($"Simulate device cancelled: {simDevice.device.Id}");
        }

        private static async Task SendDeviceEventAsync(SimDevice simDevice, DeviceData deviceData,
            DeviceClient client, CancellationToken token)
        {
            var messageString = JsonConvert.SerializeObject(deviceData);
            var message = new Microsoft.Azure.Devices.Client.Message(Encoding.UTF8.GetBytes(messageString));

            Console.WriteLine($"Simulate device: {simDevice.device.Id} : {messageString}");
            await client.SendEventAsync(message);

            await Task.Delay(Constants.simulationDelay, token);
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
    }
}
