using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RefactoringForReadability
{
    public class DateRangeTests
    {
        [Theory]
        [InlineData("10-Mar-16", null, "12-May-16", null, true)]
        [InlineData("10-Mar-16", "10-Apr-16", "12-Apr-16", "25-May-16", false)]
        [InlineData("10-Mar-16", null, "12-Apr-16", "25-Apr-16", true)]
        [InlineData("10-Mar-16", "10-Apr-16", "12-Apr-16", "25-Apr-16", false)]
        [InlineData("10-Mar-16", "10-Apr-16", "12-Apr-16", null, false)]
        [InlineData("10-May-16", "10-Apr-17", "12-Apr-15", "10-May-16", true)]
        [InlineData("10-Mar-16", "10-Apr-16", "11-Apr-16", "10-May-16", false)]
        [InlineData("10-Mar-16", "10-Apr-16", "01-Apr-16", "15-Apr-16", true)]
        [InlineData("10-Mar-16", "10-Apr-16", "28-Mar-16", "03-Apr-16", true)]
        [InlineData("10-Mar-16", null, "28-Jan-16", "10-Mar-16", true)]
        [InlineData("28-Jan-16", "10-Mar-16", "10-Mar-16", null, true)]
        [InlineData("10-Jan-2016", "10-Feb-2016", "11-Feb-2016", "10-Dec-2016", false)]
        [InlineData("10-Jan-2015", "10-Feb-2015", "20-Jan-2015", "1-Feb-2016", true)]
        [InlineData("10-Jan-2015", null, "20-Jan-2016", null,  true)]
        public void OverlappingDatesReturnsExpected(
            string startDateTime1,
            string endDateTime1,
            string startDateTime2,
            string endDateTime2,
            bool expected)
        {
            // Fixture setup
            var range1 = CreateDateRange(startDateTime1, endDateTime1);
            var range2 = CreateDateRange(startDateTime2, endDateTime2);
            // Exercise system
            var actual = range1.IsOverlapping(range2);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        private DateRange CreateDateRange(string startdateTime, string endDateTime)
        {
            DateTime? end = endDateTime == null ? (DateTime?)null : DateTime.Parse(endDateTime);
            return new DateRange(DateTime.Parse(startdateTime), end);
        }
    }
}
