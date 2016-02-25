namespace FillTheDocDb.Models
{
    using Microsoft.Azure.Search.Models;

    public static class SampleIndex
    {
        public static Index CreateIndexDefinition(string indexName)
        {
            var definition = new Index()
            {
                Name = indexName,
                Fields = new[]
                {
                    new Field("IndexKey", DataType.String) {IsKey = true, IsFilterable = true, IsSortable = true},
                    new Field("DeviceDataType", DataType.String) {IsKey = false, IsFilterable = true, IsSortable = true},
                    new Field("DeviceId", DataType.String) {IsFilterable = true, IsSortable = true},
                    new Field("UserId", DataType.String) {IsFilterable = true, IsSortable = true},
                    new Field("StartTime", DataType.DateTimeOffset) {IsFilterable = true, IsSortable = true},
                    new Field("EndTime", DataType.DateTimeOffset) {IsFilterable = true, IsSortable = true},
                    new Field("ParentDay", DataType.DateTimeOffset) {IsFilterable = true, IsSortable = true},
                    new Field("StepsTaken", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("TotalCalories", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("AverageHeartRate", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("PeakHeartRate", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("LowestHeartRate", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("TotalDistance", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("ActualDistance", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("MaxElevation", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("MinElevation", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("Speed", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("Latitude", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("Longitude", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("GeoLocation", DataType.GeographyPoint) {IsFilterable = true, IsSortable = true},
                    new Field("LocationKeywords", DataType.String) {IsFilterable = true, IsSortable = true, IsFacetable = true, IsSearchable = true},
                    new Field("LocationName", DataType.String) {IsFilterable = true, IsSortable = true, IsFacetable = true, IsSearchable = true},
                    new Field("ActiveHours", DataType.Double) {IsFilterable = true, IsSortable = true},
                    new Field("UvExposure", DataType.DateTimeOffset) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("UserAge", DataType.Double) {IsFilterable = true, IsSortable = true, IsFacetable = true},
                    new Field("UserGender", DataType.String) {IsFilterable = true, IsSortable = true, IsFacetable = true, IsSearchable = true}
                   }
            };
            return definition;
        }
    }
}
