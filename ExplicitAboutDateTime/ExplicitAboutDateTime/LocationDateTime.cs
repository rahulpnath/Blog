using System;

namespace ExplicitAboutDateTime
{
    public class LocationDateTime
    {
        public Location Location { get; private set; }
        public DateTime DateTimeInUTC { get; private set; }
        public DateTimeOffset DateTimeInLocation { get; private set; }

        public LocationDateTime(Location location, DateTime dateTimeUTC)
        {
            if (location == null)
                throw new ArgumentNullException(nameof(location));

            if (dateTimeUTC == null)
                throw new ArgumentNullException(nameof(dateTimeUTC));

            if (dateTimeUTC.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date Time not in UTC");

            Location = location;
            DateTimeInUTC = dateTimeUTC;
            DateTimeInLocation = TimeAtLocation(Location);
        }

        public static LocationDateTime AtLocation(DateTime locationDateTime, Location location)
        {
            if (locationDateTime.Kind != DateTimeKind.Unspecified)
                throw new ArgumentException("DateTimeKind should be unspecified");

            var utcTime = TimeZoneInfo.ConvertTimeToUtc(locationDateTime, location.TimeZoneInfo);
            return new LocationDateTime(location, utcTime);
        }

        public DateTimeOffset TimeAtLocation(Location location)
        {
            return TimeZoneInfo.ConvertTime((DateTimeOffset)DateTimeInUTC, location.TimeZoneInfo);
        }

        public override bool Equals(object obj)
        {
            var objAsLocationDateTime = obj as LocationDateTime;
            if ((System.Object)objAsLocationDateTime == null)
                return false;

            return objAsLocationDateTime.DateTimeInUTC == DateTimeInUTC;
        }

        public override int GetHashCode()
        {
            return DateTimeInUTC.GetHashCode();
        }
    }
}
