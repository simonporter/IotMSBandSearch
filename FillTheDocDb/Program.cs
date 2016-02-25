namespace FillTheDocDb
{
    using Microsoft.Azure.Search;
    using Microsoft.ServiceBus.Messaging;
    using Models;
    using System;
    using Utils;

    class Program
    {
        static void Main(string[] args)
        {
            // From time is how many minutes back from now we will read the IOT Hub to get messages
            DateTime fromTime = DateTime.UtcNow.AddMinutes(-600);

            // Set to true to read from the sample data file instead of the IoT Hub.
            bool useTextFile = false;

            // Delete and re-create the index
            bool reCreateIndex = false;

            // Create the service client
            SearchServiceClient serviceClient = new SearchServiceClient(Constants.SearchServiceName, new SearchCredentials(Constants.ApiKey));

            if (reCreateIndex)
            {
                // Delete the index if it already exists.
                IndexUtils.DeleteIndexIfExists(serviceClient, Constants.IndexName);

                // Create the new index definition
                var indexDef = SampleIndex.CreateIndexDefinition(Constants.IndexName);

                // Create the index
                IndexUtils.CreateIndex(serviceClient, Constants.IndexName, indexDef);
            }
           

            //Subscribe to the IotHub and get the data
            var eventHubClient = EventHubClient.CreateFromConnectionString(Constants.Hub2ConnectionString, Constants.IotHubD2CEndpoint);

            var d2cPartitions = eventHubClient.GetRuntimeInformation().PartitionIds;

            if (useTextFile)
            {
                // Read the text file and upload the documents to the index.
                var dataToBeSentToIndex = Utils.Utils.GetModelList(Utils.Utils.ReadFileToList(Constants.SampleTextFile));
                IndexUtils.UploadDcuments(serviceClient, dataToBeSentToIndex, Constants.IndexName);
            }

            else
            {
                // Read the messages from the IoT Hub
                foreach (string partition in d2cPartitions)
                {
                    // Get messages from the hub
                    IotHubMessageHelper.GetHubMessagesWriteDocumentsTask(eventHubClient, serviceClient, partition, fromTime).Wait();
                }
            }

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();
        }
    }
}
