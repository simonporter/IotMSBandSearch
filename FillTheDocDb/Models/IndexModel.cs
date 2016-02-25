namespace FillTheDocDb.Models
{
    using Newtonsoft.Json;
    using System;
    using Microsoft.Spatial;

    public class IndexModel
    {
        public string IndexKey { get; set; }
        public string DeviceDataType { get; set; }
        public string DeviceId { get; set; }
        public string UserId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime ParentDay { get; set; }
        public double StepsTaken { get; set; }
        public double TotalCalories { get; set; }
        public double AverageHeartRate { get; set; }
        public double PeakHeartRate { get; set; }
        public double LowestHeartRate { get; set; }
        public double TotalDistance { get; set; }
        public double ActualDistance { get; set; }
        public double MaxElevation { get; set; }
        public double MinElevation { get; set; }
        public double Speed { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public GeographyPoint GeoLocation { get; set; }
        public string LocationName { get; set; }
        public string LocationKeywords { get; set; }
        public double ActiveHours { get; set; }
        public DateTime UvExposure { get; set; }
        public double UserAge { get; set; }
        public string UserGender { get; set; }

        public static IndexModel NewIndexModel(string data)
        {
            IndexModel model = new IndexModel();
            var x = JsonConvert.DeserializeObject<IndexModel>(data);
            x.IndexKey = x.DeviceDataType + "_" + x.DeviceId;
            model.ActiveHours = x.ActiveHours;
            model.ActualDistance = x.ActualDistance;
            model.AverageHeartRate = x.AverageHeartRate;
            model.DeviceDataType = x.DeviceDataType;
            model.DeviceId = x.DeviceId;
            model.EndTime = x.EndTime;
            model.IndexKey = x.IndexKey;
            model.LocationKeywords = x.LocationKeywords;
            model.LocationName = x.LocationName;
            model.Latitude = x.Latitude;
            model.Longitude = x.Longitude;
            model.GeoLocation = GeographyPoint.Create(x.Latitude, x.Longitude);
            model.LowestHeartRate = x.LowestHeartRate;
            model.MaxElevation = x.MaxElevation;
            model.MinElevation = x.MinElevation;
            model.ParentDay = x.ParentDay;
            model.PeakHeartRate = x.PeakHeartRate;
            model.Speed = x.Speed;
            model.StartTime = x.StartTime;
            model.StepsTaken = x.StepsTaken;
            model.TotalCalories = x.TotalCalories;
            model.TotalDistance = x.TotalDistance;
            model.UserId = x.UserId;
            model.UvExposure = x.UvExposure;
            model.UserAge = x.UserAge;
            model.UserGender = x.UserGender;

            return model;
        }
    }
}
