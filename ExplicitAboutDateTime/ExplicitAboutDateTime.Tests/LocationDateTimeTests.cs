using System;
using Xunit;

namespace ExplicitAboutDateTime.Tests
{
    public class LocationDateTimeTests
    {

        [Fact]
        public void LocationDateTimeSetsTimeAsExpected()
        {
            var timeInUTC = new DateTime(2016, 03, 23, 4, 30, 0, DateTimeKind.Utc);

            var sut = new LocationDateTime(GetTestLocation(), timeInUTC);
            Assert.Equal(sut.DateTimeInUTC, timeInUTC);
        }

        [Fact]
        public void LocationDateTimeThrowsExceptionForLocalDateTimeKind()
        {
            var timeInUTC = new DateTime(2016, 03, 23, 4, 30, 0, DateTimeKind.Local);
            Assert.Throws<ArgumentException>(() => new LocationDateTime(GetTestLocation(), timeInUTC));
        }

        [Fact]
        public void LocationDateTimeThrowsExceptionForUnspecifiedDateTimeKind()
        {
            var timeInUTC = new DateTime(2016, 03, 23, 4, 30, 0, DateTimeKind.Unspecified);
            Assert.Throws<ArgumentException>(() => new LocationDateTime(GetTestLocation(), timeInUTC));
        }

        private Location GetTestLocation()
        {
            return new Location("TRV", "Trivandrum", TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
    }
}
