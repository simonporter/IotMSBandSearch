using Microsoft.Azure.Search;
using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ElasticSearchMSBandWeb
{
    public class BandFacetSearch
    {
        private static SearchServiceClient _searchClient;
        private static SearchIndexClient _indexClient;
        private static string IndexName = "hckindex";

        public static string errorMessage;

        static BandFacetSearch()
        {
            try
            {
                string searchServiceName = ConfigurationManager.AppSettings["SearchServiceName"];
                string apiKey = ConfigurationManager.AppSettings["SearchServiceApiKey"];

                // Create an HTTP reference to the catalog index
                _searchClient = new SearchServiceClient(searchServiceName, new SearchCredentials(apiKey));
                _indexClient = _searchClient.Indexes.GetClient(IndexName);
            }
            catch (Exception e)
            {
                errorMessage = e.Message.ToString();
            }
        }

        public DocumentSearchResult Search(
            string searchText, 
            string locationNameFacet,
            string totalCaloriesFacet, 
            int currentPage)
        {
            // Execute search based on query string
            try
            {
                SearchParameters sp = new SearchParameters()
                {
                    SearchMode = SearchMode.Any,
                    Top = 10,
                    Skip = currentPage - 1,
                    // Select all columns
                    Select = new List<String>() { "*" },
                    // Add count
                    IncludeTotalResultCount = true,
                    // Add facets
                    Facets = new List<String>() { "TotalCalories,interval:10", "LocationName" },
                };

                // Add filtering
                string filter = null;
                if (locationNameFacet != "")
                {
                    filter = "LocationName eq '" + locationNameFacet + "'";
                }
                if (totalCaloriesFacet != "")
                {
                    if (filter != null) { filter += " and "; }
                    filter = "TotalCalories ge " + totalCaloriesFacet + " and TotalCalories lt " + (Convert.ToInt32(totalCaloriesFacet) + 10).ToString();
                }

                sp.Filter = filter;

                return _indexClient.Documents.Search(searchText, sp);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

        public Document LookUp(string id)
        {
            try
            {
                return _indexClient.Documents.Get($"BandTest2_{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error querying index: {0}\r\n", ex.Message.ToString());
            }
            return null;
        }

    }
}