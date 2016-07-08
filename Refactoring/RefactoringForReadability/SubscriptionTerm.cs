using System;

namespace RefactoringForReadability
{
    public class SubscriptionTerm
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public DateRange TermPeriod
        {
            get
            {
                return new DateRange(StartDate, EndDate);
            }
        }

        public bool IsOverlapping(SubscriptionTerm term)
        {
            return TermPeriod.IsOverlapping(term.TermPeriod);
        }
    }
}