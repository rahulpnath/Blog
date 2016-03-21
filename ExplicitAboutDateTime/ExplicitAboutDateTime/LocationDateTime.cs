using System;

namespace ExplicitAboutDateTime
{
    public class LocationDateTime
    {
        private DateTime internalDateTimeUTC;

        public LocationDateTime(DateTime dateTimeUTC)
        {
            if (dateTimeUTC == null)
                throw new ArgumentNullException(nameof(dateTimeUTC));

            if (dateTimeUTC.Kind != DateTimeKind.Utc)
                throw new ArgumentException("Date Time not in UTC");

            internalDateTimeUTC = dateTimeUTC;
        }

        public static LocationDateTime AtLocation(DateTime locationDateTime, Location location)
        {
            locationDateTime = DateTime.SpecifyKind(locationDateTime, DateTimeKind.Unspecified);
            var utcTime = TimeZoneInfo.ConvertTimeToUtc(locationDateTime, location.TimeZoneInfo);
            return new LocationDateTime(utcTime);
        }

        public override bool Equals(object obj)
        {
            var objAsLocationDateTime = obj as LocationDateTime;
            if ((System.Object)objAsLocationDateTime == null)
                return false;

            return objAsLocationDateTime.internalDateTimeUTC == internalDateTimeUTC;
        }

        public override int GetHashCode()
        {
            return internalDateTimeUTC.GetHashCode();
        }
    }
}
