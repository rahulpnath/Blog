using System;
using Xunit;

namespace ExplicitAboutDateTime.Tests
{
    public class LocationDateTimeTests
    {

        [Fact]
        public void LocationDateTimeSetsUTCTimeAsExpected()
        {
            var timeInUTC = new DateTime(2016, 03, 23, 5, 30, 0, DateTimeKind.Utc);

            var sut = new LocationDateTime(GetTestLocation(), timeInUTC);
            Assert.Equal(sut.DateTimeInUTC, timeInUTC);
        }

        [Fact]
        public void LocationDateTimeSetsLocationTimeAsExpected()
        {
            var timeInUTC = new DateTime(2016, 03, 23, 5, 30, 0, DateTimeKind.Utc);
            var locationTime = new DateTimeOffset(2016, 03, 23, 11, 0, 0, new TimeSpan(5, 30, 0));
               
            var sut = new LocationDateTime(GetTestLocation(), timeInUTC);
            Assert.Equal(sut.DateTimeInLocation, locationTime);
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


        [Fact]
        public void AtLocationForUnspecifiedDateTimeKindReturnsExpected()
        {
            var expectedUTC = new DateTime(2016, 03, 22, 23, 0, 0, DateTimeKind.Utc);
            var timeAtLocation = new DateTime(2016, 03, 23, 4, 30, 0, DateTimeKind.Unspecified);
            var sut = LocationDateTime.AtLocation(timeAtLocation, GetTestLocation());

            Assert.Equal(expectedUTC, sut.DateTimeInUTC);
        }

        [Fact]
        public void AtLocationForNonUnspecifiedDateTimeKindThrowsException()
        {
            Assert.Throws<ArgumentException>(() => LocationDateTime.AtLocation(DateTime.UtcNow, GetTestLocation()));
            Assert.Throws<ArgumentException>(() => LocationDateTime.AtLocation(DateTime.Now, GetTestLocation()));
        }

        private Location GetTestLocation()
        {
            return new Location("TRV", "Trivandrum", TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
    }
}
