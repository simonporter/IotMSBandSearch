namespace FillTheDocDb.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Azure.Search;
    using Microsoft.ServiceBus.Messaging;
    using System.Threading;
    using System.Threading.Tasks;
    using Models;

    public class IotHubMessageHelper
    {
        public async static Task GetHubMessagesWriteDocumentsTask(EventHubClient eventHubClient, SearchServiceClient searchServiceClient, string partition, DateTime fromTime, CancellationToken cancellation)
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup().CreateReceiver(partition, fromTime);
            while (true)
            {
                var listToSend = new List<IndexModel>();
                Console.WriteLine("Waiting for data from partition: {0}", partition);
                EventData eventData = await eventHubReceiver.ReceiveAsync();
                if (eventData == null) continue;

                string data = Encoding.UTF8.GetString(eventData.GetBytes());
                listToSend.Add(IndexModel.NewIndexModel(data));
                Console.WriteLine(string.Format("Message received. Partition: {0} Data: '{1}'", partition, data));

                // Write the documents to the index
                IndexUtils.UploadDcuments(searchServiceClient, listToSend, Constants.IndexName);
            }
        }
    }
}
