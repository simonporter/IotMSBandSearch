using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SearchTests
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FillTheDocDb;
    using FillTheDocDb.Models;
    using Microsoft.Azure.Search;
    using Microsoft.Azure.Search.Models;

    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            SearchServiceClient serviceClient = new SearchServiceClient(Constants.SearchServiceName, new SearchCredentials(Constants.ApiKey));

            SearchIndexClient indexClient = serviceClient.Indexes.GetClient(Constants.IndexName);

            List<string> facetList = new List<string>();
            facetList.Add("TotalCalories");
            facetList.Add("UserGender");
            facetList.Add("UserAge");

            var x = await serviceClient.Indexes.GetWithHttpMessagesAsync(Constants.IndexName);

            SearchDocuments(indexClient, searchText: "*", facetList: facetList, filter: "UserGender eq 'Male'&LocationKeywords");

            SearchDocuments(indexClient, searchText: "Bellevue");

            SearchDocuments(indexClient, searchText: "*", facetList: facetList, filter: "UserGender eq 'Male'");

        }

        private static void SearchDocuments(SearchIndexClient indexClient, string searchText, List<string> facetList = null,  string filter = null)
        {
            // Execute search based on search text and optional filter
            var sp = new SearchParameters();

            if (!String.IsNullOrEmpty(filter))
            {
                sp.Filter = filter;
                sp.Facets = facetList;
            }

            DocumentSearchResult<IndexModel> response = indexClient.Documents.Search<IndexModel>(searchText, sp);
            Console.WriteLine("Matched {0}", response.Results.Count);
            foreach (SearchResult<IndexModel> result in response.Results)
            {
                Console.WriteLine("DocId: {0} Location: {1} Gender: {2} ",result.Document.IndexKey, result.Document.LocationName, result.Document.UserGender);
            }
        }
    }
}
