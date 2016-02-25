namespace ElasticSearchMSBand
{
    using System;

    public class DeviceData
    {
        public string deviceDataType = "BandTest2";
        public string deviceId = "";
        public string userId = Guid.NewGuid().ToString("N");
        public DateTime startTime = new DateTime();
        public DateTime endTime = new DateTime();
        public DateTime parentDay = new DateTime();
        public string period = "Segment";
        public int stepsTaken = 0;
        public int totalCalories = 0;
        public int averageHeartRate = 80;
        public int peakHeartRate = 80;
        public int lowestHeartRate = 80;
        public int totalDistance = 0;
        public int actualDistance = 0;
        public int maxElevation = 250;
        public int minElevation = 250;
        public int speed = 0;
        public double latitude = 0.0;
        public double longitude = 0.0;
        public string locationKeywords = "";
        public string locationName = "";
        public string userGender = "";
        public int userAge = 0;

        public double activeHours = 0.0;
        public DateTime uvExposure = new DateTime();
    }
}
