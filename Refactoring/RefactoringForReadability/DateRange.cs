using System;

namespace RefactoringForReadability
{
    public class DateRange
    {
        public DateTime StartDate { get; private set; }

        public DateTime? EndDate { get; private set; }

        public DateRange(DateTime startDate, DateTime? endDate)
        {
            if (endDate.HasValue && endDate.Value < startDate)
                throw new ArgumentException("End date cannot be less than start Date");

            StartDate = startDate;
            EndDate = endDate;
        }

        public bool IsOverlapping(DateRange dateRange)
        {
            if (!EndDate.HasValue && !dateRange.EndDate.HasValue)
                return true;

            if (!EndDate.HasValue)
                return StartDate <= dateRange.EndDate;

            if (!dateRange.EndDate.HasValue)
                return dateRange.StartDate <= EndDate;

            return StartDate <= dateRange.EndDate
                && dateRange.StartDate <= EndDate;
        }

    }
}
