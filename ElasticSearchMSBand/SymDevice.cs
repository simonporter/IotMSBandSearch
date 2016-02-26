namespace ElasticSearchMSBand
{
    using System;
    using Microsoft.Azure.Devices;

    public class SimDevice
    {
        private Random rand = new Random();

        public SimDevice(Device device)
        {
            bool flip = (rand.Next(2) == 0);
            this.device = device;
            var heartRate = rand.Next(60, 130);
            var elevation = rand.Next(250, 4000);
            int locationRnd = rand.Next(6);
            double longLoc = 0;
            double latLoc = 0;
            string keywordsLoc = "";
            string locationName = "";
            switch (locationRnd)
            {
                case 1: // safeco
                    longLoc = -122.320892;
                    latLoc = 47.593628;
                    keywordsLoc = "safco field, seattle, washington, wa, mariners";
                    locationName = "Safeco Field";
                    break;
                case 2: // yankee
                    longLoc = -73.926390;
                    latLoc = 40.829170;
                    keywordsLoc = "yankees, nyc, ny, new york, manhattan, nj, new jersey";
                    locationName = "Yankee Stadium";
                    heartRate = heartRate + 20;
                    break;
                case 3: // close to safeco
                    longLoc = -122.260892;
                    latLoc = 47.593628;
                    keywordsLoc = "bellevue, wa, washington";
                    locationName = "Bellevue";
                    break;
                case 4: // close to yankee
                    longLoc = -73.965390;
                    latLoc = 40.829170;
                    keywordsLoc = "ny, new york, nj, new jersey";
                    locationName = "Hoboken";
                    break;
                case 5: // Florida
                    longLoc = -73.965390;
                    latLoc = 25.829170;
                    keywordsLoc = "florida, miami, fl, marlins";
                    locationName = "Miami";
                    break;
                default: // somewhere else
                    longLoc = 11.320892;
                    latLoc = -23.593628;
                    keywordsLoc = "far away";
                    locationName = "Madagascar";
                    heartRate = heartRate - 15;
                    break;
            }

            this.CurrentData = new DeviceData()
            {
                deviceId = device.Id,
                speed = rand.Next(0, 15),
                averageHeartRate = heartRate,
                peakHeartRate = heartRate + 5,
                lowestHeartRate = heartRate - 5,
                maxElevation = elevation,
                minElevation = elevation - rand.Next(1, 50),
                totalCalories = rand.Next(0, 50),
                latitude = latLoc,
                longitude = longLoc,
                locationKeywords = keywordsLoc,
                locationName = locationName,
                userAge = rand.Next(13, 80),
                userGender = flip ? "Male" : "Female"
            };
        }

        public Device device;
        public DeviceData CurrentData;
    }

}
