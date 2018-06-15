using System;

namespace FCore.Foundations
{
    public class DateRange
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public DateRange(DateTime startDate, DateTime endDate)
        {
            if (startDate <= endDate)
            {
                StartDate = startDate;
                EndDate = endDate;
            }
            else
            {
                StartDate = endDate;
                EndDate = startDate;
            }
        }

        public DateRange(DateTime startDate, TimeSpan interval)
        {
            StartDate = startDate;
            EndDate = startDate.Add(interval);
        }

        public TimeSpan TimeSpan 
        { 
            get
            {
                return EndDate - StartDate;
            }
        }

        public bool IsWithinDateRange(DateTime date)
        {
            return ((StartDate <= date) && (date <= EndDate));
        }
    
    }
}
