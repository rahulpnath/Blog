using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void TryCreateDateInUTCReturnsAsExpected(string invalidDate, bool expected)
        {
            LocationDateTime date;
            var actual = LocationDateTime.TryCreateUTC(invalidDate, out date);
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("10 Apr 2010", true)]
        [InlineData("10 04 2010", true)]
        public void TryCreateDateInUTCSetsOutDateAsExpected(string invalidDate, bool expected)
        {
            LocationDateTime locationDateTime;
            var actual = LocationDateTime.TryCreateUTC(invalidDate, out locationDateTime);
            Assert.Equal(expected, locationDateTime != null);
        }
    }
}
