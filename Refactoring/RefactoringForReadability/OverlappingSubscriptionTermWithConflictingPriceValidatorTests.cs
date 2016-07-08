using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace RefactoringForReadability
{
    public class OverlappingSubscriptionTermWithConflictingPriceValidatorTests
    {
        [Theory]
        [InlineData("10-Jan-2015", "10-Feb-2015", 1, "10-Jan-2016", "10-Feb-2016", 2, false)]
        [InlineData("10-Jan-2015", "10-Feb-2015", 1, "20-Jan-2015", "1-Feb-2016", 2, true)]
        [InlineData("10-Jan-2015", "10-Feb-2015", 1, "20-Jan-2015", "1-Feb-2016", 1, false)]
        [InlineData("10-Jan-2015", null, 1, "20-Jan-2016", "1-Feb-2016", 1, false)]
        [InlineData("10-Jan-2015", null, 1, "20-Jan-2016", "1-Feb-2016", 2, true)]
        [InlineData("10-Jan-2016", "10-Feb-2016", 1, "20-Feb-2016", null, 2, false)]
        [InlineData("10-Jan-2015", null, 3, "20-Jan-2016", null, 2, true)]
        [InlineData("10-Jan-2015", null, 1, "20-Jan-2016", null, 1, false)]
        [InlineData("10-Jan-2016", null, 1, "12-Dec-2016", null, 2, true)]
        [InlineData("10-Jan-2016", "10-Feb-2016", 1, "12-Feb-2016", "25-Dec-2016", 2, false)]
        [InlineData("10-Jan-2016", null, 1, "12-Feb-2016", "25-Feb-16", 2, true)]
        [InlineData("10-Dec-2016", "10-Feb-2017", 1, "12-Feb-2015", "10-Dec-2016", 2, true)]
        [InlineData("10-Jan-2016", "10-Feb-2016", 1, "11-Feb-2016", "10-Dec-2016", 2, false)]
        [InlineData("10-Jan-2016", "10-Feb-2016", 1, "01-Feb-2016", "15-Feb-2016", 2, true)]
        [InlineData("10-Jan-2016", "10-Feb-2016", 1, "28-Jan-2016", "03-Feb-2016", 2, true)]
        [InlineData("10-Jan-2016", null, 1, "28-Aug-2015", "10-Jan-2016", 2, true)]
        [InlineData("28-Aug-2015", "10-Jan-2016", 1, "10-Jan-2016", null, 2, true)]
        public void ValiadteReturnsExpected(
            string startDate1, string endDate1, double price1,
            string startDate2, string endDate2, double price2,
            bool expected )
        {
            // Fixture setup
            var subscription = new Subscription();
            var term1 = createTerm(startDate1, endDate1, price1);
            var term2 = createTerm(startDate2, endDate2, price2);
            subscription.Terms.Add(term1);
            subscription.Terms.Add(term2);
            // Exercise system
            var sut = new RefactoredOverlappingSubscriptionTermWithConflictingPriceValidator();
            var actual = sut.Validate(subscription);
            // Verify outcome
            Assert.Equal(expected, actual);
            // Teardown
        }

        private SubscriptionTerm createTerm(string startDate, string endDate, double price)
        {
            var start = DateTime.Parse(startDate);
            DateTime? end;
            DateTime temp;
            end = DateTime.TryParse(endDate, out temp) ? temp : (DateTime?)null;

            return new SubscriptionTerm() { Price = price, StartDate = start, EndDate = end };
        }
    }
}
