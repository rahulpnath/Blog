using System;

namespace ExplicitAboutDateTime
{
    public class Location
    {
        public string LocationCode { get; private set; }
        public string LocationName { get; private set; }
        public TimeZoneInfo TimeZoneInfo { get; private set; }

        public Location(string locationCode, string locationName, TimeZoneInfo timeZoneInfo)
        {
            // Skipped validations
            this.LocationCode = locationCode;
            this.LocationName = locationName;
            this.TimeZoneInfo = timeZoneInfo;

        }
    }
}
