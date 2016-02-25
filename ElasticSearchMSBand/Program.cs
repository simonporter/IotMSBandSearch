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

    internal class Program
    {
        private static Random rand = new Random();

        private static void Main(string[] args)
        {
            Console.WriteLine($"Using ConnectionString: {Constants.ConnectionString}");

            // Bypass https cert validation so we can use fiddler
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;
            ServicePointManager.Expect100Continue = false;

            CancellationTokenSource cts = new CancellationTokenSource();
            List<SimDevice> devices = new List<SimDevice>(Constants.NumDevices);
            var token = cts.Token;
            try
            {
                devices = DeviceRegistrar.RegisterDevicesAsync(Constants.NumDevices, token).Result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception adding Devices: {e}");
            }

            Task simulations = null;
            try
            {
                simulations = DeviceSimulator.SimulateDevices(devices, cts.Token);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception simulating Devices: {e}");
                Console.WriteLine($"Simulations exception: {simulations?.Exception}");
            }

            Console.WriteLine("Hit Enter to Exit!!");
            Console.ReadLine();
            Console.WriteLine("##### Stopping simulation (wait 1 second)");
            cts.Cancel();
            Thread.Sleep(1000);

            cts.Dispose();
            Console.WriteLine("Exiting");
        }
    }
}
