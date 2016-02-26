using ElasticSearchMSBandWeb.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticSearchMSBandWeb.Controllers
{
    public class HomeController : Controller
    {
        private BandFacetSearch _bandFacetSearch = new BandFacetSearch();

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DeviceDetail()
        {
            return View();
        }

        public ActionResult Search(
            string q = "", 
            string totalCaloriesFacet = "", 
            string locationNameFacet = "", 
            string heartRateFacet = "", 
            int currentPage = 0)
        {
            // If blank search, assume they want to search everything
            if (string.IsNullOrWhiteSpace(q))
                q = "*";

            var response = _bandFacetSearch.Search(q, locationNameFacet, totalCaloriesFacet, heartRateFacet, currentPage);
            return new JsonResult
            {
                // ***************************************************************************************************************************
                // If you get an error here, make sure to check that you updated the SearchServiceName and SearchServiceApiKey in Web.config
                // ***************************************************************************************************************************

                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = new MSBandDevice() { Results = response.Results, Facets = response.Facets, Count = Convert.ToInt32(response.Count) }
            };
        }

        public ActionResult LookUp(string id)
        {
            // Take a key ID and do a lookup to get the device details
            if (id != null)
            {
                var response = _bandFacetSearch.LookUp(id);
                return new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new MSBandDeviceLookup() { Result = response }
                };
            }
            else
            {
                return null;
            }

        }

    }
}
