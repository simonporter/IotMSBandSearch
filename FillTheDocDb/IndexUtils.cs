namespace FillTheDocDb
{
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    public static class IndexUtils
    {
        public static void DeleteIndexIfExists(SearchServiceClient serviceClient, string indexName)
        {
            if (serviceClient.Indexes.Exists(indexName))
            {
                serviceClient.Indexes.Delete(indexName);
                Console.WriteLine("Successfully deleted index {0}", indexName);
            }
        }

        public static void CreateIndex(SearchServiceClient serviceClient, string indexName, Index indexDefinition)
        {
            serviceClient.Indexes.Create(indexDefinition);
            Console.WriteLine("Successfully created index {0}", indexName);
        }

        public static void UploadDcuments(SearchServiceClient serviceClient, List<IndexModel> documents, string indexName)
        {
            var indexClient = serviceClient.Indexes.GetClient(indexName);
            try
            {
                var batch = IndexBatch.Upload(documents);
                indexClient.Documents.Index(batch);
            }
            catch (IndexBatchException e)
            {
                // Sometimes when your Search service is under load, indexing will fail for some of the documents in
                // the batch. Depending on your application, you can take compensating actions like delaying and
                // retrying. For this simple demo, we just log the failed document keys and continue.
                Console.WriteLine(
                    "Failed to index some of the documents: {0}",
                    String.Join(", ", e.IndexingResults.Where(r => !r.Succeeded).Select(r => r.Key)));
            }
            // Wait a while for indexing to complete.
            Thread.Sleep(2000);
        }
    }
}
