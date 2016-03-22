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
            if (string.IsNullOrEmpty(LocationCode))
                throw new ArgumentNullException(nameof(LocationCode));

            if (string.IsNullOrEmpty(locationName))
                throw new ArgumentNullException(nameof(locationName));

            if (timeZoneInfo == null)
                throw new ArgumentNullException(nameof(timeZoneInfo));

            this.LocationCode = locationCode;
            this.LocationName = locationName;
            this.TimeZoneInfo = timeZoneInfo;

        }
    }
}
