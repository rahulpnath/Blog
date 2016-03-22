using System;

namespace ExplicitAboutDateTime
{
    public class LocationDateTime
    {
        public Location Location { get; private set; }
        public DateTime DateTimeInUTC { get; private set; }

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
        }

        public static LocationDateTime AtLocation(DateTime locationDateTime, Location location)
        {
            locationDateTime = DateTime.SpecifyKind(locationDateTime, DateTimeKind.Unspecified);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(locationDateTime, location.TimeZoneInfo);
            return new LocationDateTime(location, utcTime);
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
