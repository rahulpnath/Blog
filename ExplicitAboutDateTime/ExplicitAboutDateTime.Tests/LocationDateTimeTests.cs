using System;
using Xunit;

namespace ExplicitAboutDateTime.Tests
{
    public class LocationDateTimeTests
    {
        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("10 Apr 2010", true)]
        [InlineData("10 04 2010", true)]
        [InlineData("10 04 2010 00:00:00", true)]
        [InlineData("10 04 2010 00:00:01", false)]
        public void TryCreateDateInUTCReturnsAsExpected(string invalidDate, bool expected)
        {
            LocationDateTime date;
            var actual = LocationDateTime.TryCreateDateFromUTC(invalidDate, out date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("10 Apr 2010", true)]
        [InlineData("10 04 2010", true)]
        [InlineData("10 04 2010 00:00:00", true)]
        [InlineData("10 04 2010 00:00:01", false)]
        [InlineData("10 04 2010 00:00:00 +00:00", true)]
        [InlineData("10 04 2010 00:00:00 +01:00", false)]
        public void TryCreateDateInUTCSetsOutDateAsExpected(string invalidDate, bool expected)
        {
            LocationDateTime locationDateTime;
            var actual = LocationDateTime.TryCreateDateFromUTC(invalidDate, out locationDateTime);
            Assert.Equal(expected, locationDateTime != null);
        }

        [Theory]
        [InlineData("10 Apr 2010", null, false)]
        [InlineData("10 Apr 2010", "11 Apr 2010", false)]
        [InlineData("10 Apr 2010", "10 Apr 2010", true)]
        [InlineData("10 Apr 2010", "04 10 2010", true)]
        [InlineData("10 Apr 2010", "10 Apr 2010 00:00:00", true)]
        [InlineData("10 Apr 2010", "04/10/2010 00:00:00", true)]
        [InlineData("10 Apr 2010", "10 Apr 2010 00:00:01", false)]
        public void SutOverridesEqualsAsExpected(
            string aLocationDate,
            string anotherLocationDate,
            bool expected)
        {
            LocationDateTime aLocationDateTime;
            LocationDateTime.TryCreateDateFromUTC(aLocationDate, out aLocationDateTime);

            LocationDateTime anotherLocationDateTime;
            LocationDateTime.TryCreateDateFromUTC(anotherLocationDate, out anotherLocationDateTime);

            Assert.Equal(expected, aLocationDateTime.Equals(anotherLocationDateTime));
        }

        [Theory]
        [InlineData("10 Apr 2010", "11 Apr 2010", false)]
        [InlineData("10 Apr 2010", "10 Apr 2010", true)]
        [InlineData("10 Apr 2010", "04 10 2010", true)]
        [InlineData("10 Apr 2010", "10 Apr 2010 00:00:00", true)]
        [InlineData("10 Apr 2010", "04/10/2010 00:00:00", true)]
        public void SutOverridesGetHashCodeAsExpected(
            string aLocationDate,
            string anotherLocationDate,
            bool expected)
        {
            LocationDateTime aLocationDateTime;
            LocationDateTime.TryCreateDateFromUTC(aLocationDate, out aLocationDateTime);

            LocationDateTime anotherLocationDateTime;
            LocationDateTime.TryCreateDateFromUTC(anotherLocationDate, out anotherLocationDateTime);

            Assert.Equal(expected, aLocationDateTime.GetHashCode().Equals(anotherLocationDateTime.GetHashCode()));
        }

        [Theory]
        [InlineData("10 Apr 2015")]
        [InlineData("04 10 2015")]
        [InlineData("10 Apr 2015 00:00:00")]
        public void GetUTCDateTimeHasKindUTC(string aLocationDate)
        {
            LocationDateTime sut;
            Assert.True(LocationDateTime.TryCreateDateFromUTC(aLocationDate, out sut));
            var actual = sut.GetUTCDateTime();
            Assert.Equal(DateTimeKind.Utc, actual.Kind);
        }

        [Theory]
        [InlineData("10 Apr 2015", "TRV","10 Apr 2015")]
        [InlineData("10 Apr 2015", "SYD", "10 Apr 2015")]
        [InlineData("10 Apr 2015", "SEA", "09 Apr 2015")]
        public void GetDateTimeAtLocationReturnsExpected(string aLocationDate, string locationCode, string expected)
        {

        }
    }
}
