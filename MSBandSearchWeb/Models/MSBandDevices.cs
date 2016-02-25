using Microsoft.Azure.Search.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElasticSearchMSBandWeb.Models
{
    public class MSBandDevice
    {
        public FacetResults Facets { get; set; }
        public IList<SearchResult> Results { get; set; }
        public int? Count { get; set; }
    }

    public class MSBandDeviceLookup
    {
        public Document Result { get; set; }
    }

}