using System;

namespace Parking.Library
{
    public class HourlyCalculator : IDurationCalculator
    {
        public HourlyCalculator(int startOfDayHour, int endOfDayHour)
        {
            this.startOfDayHour = startOfDayHour;
            this.endOfDayHour = endOfDayHour;
        }

        /// <summary>
        /// Given a start and end time. determine business hours between those dates.
        /// Alogorithm: get days and then trim any partial hours at each end of the stay
        /// </summary>
        public double GetTotalChargeableQuantity(DateTime startDateTime, DateTime endDateTime)
        {
            // throw exception if end is before start
            if (endDateTime < startDateTime)
                throw new ArgumentException("end cannot be less than start");

            var businessDays = GetBusinessDays(startDateTime, endDateTime);
            
            // parked at weekend. No charge
            if (businessDays == 0) { 
                return 0; 
            }

            var timeAsMinutes = BuildBoundedMinutesTimeSpan(startDateTime, endDateTime);

            // just one day, same day in and out, calculate any adjusted hours
            if (businessDays == 1) 
            {
                // get hours, round up
                var singleDayHours = (double)Math.Ceiling(
                    (decimal)(timeAsMinutes.EndTimeMinutes - timeAsMinutes.StartTimeMinutes)
                    / 60);
                
                return singleDayHours > 0 ? singleDayHours : 1; // minimum one hour charge
            }

            var multiDayHours = CalculateMultipleDaysTotalHours(startDateTime, endDateTime, businessDays, timeAsMinutes);
            return multiDayHours;
        }

        /// <summary>
        /// this is the business / chargeable days. days that do not imclude sat and sunday.
        /// see here https://alecpojidaev.wordpress.com/2009/10/29/work-days-calculation-with-c/
        /// for algorithm. See unit tests
        /// </summary>
        public int GetBusinessDays(DateTime startDateTime, DateTime endDateTime)
        {
            if (endDateTime < startDateTime)
            {
                throw new ArgumentException("End date cannot be before start date");
            }

            var workDaysMultiplier = (endDateTime - startDateTime).TotalDays * 5; // ie. 35
            var weekendDaysMultiplier = (startDateTime.DayOfWeek - endDateTime.DayOfWeek) * 2; // ie. 14  . 35-14 =21

            double workDays = 1 + ((workDaysMultiplier - weekendDaysMultiplier) / 7);  // ie. 3 + 1 = 4

            if (endDateTime.DayOfWeek == DayOfWeek.Saturday) workDays--; // adjust
            if (startDateTime.DayOfWeek == DayOfWeek.Sunday) workDays--; // adjust

            return (int)Math.Round(workDays);
        }

        private readonly int startOfDayHour;

        private readonly int endOfDayHour;

        /// <summary>
        /// a timespan that does not pass the business day bounderies. this is for calculation only
        /// </summary>
        private MinutesTimeSpan BuildBoundedMinutesTimeSpan(DateTime startDateTime, DateTime endDateTime)
        {
            var startOfDayMinutes = this.startOfDayHour * 60; // aka 8am in original spec
            var endOfDayMinutes = this.endOfDayHour * 60; // aka 6pm

            return new MinutesTimeSpan
            {
                StartTimeMinutes = AsMinutes(startDateTime) >= startOfDayMinutes
                    ? AsMinutes(startDateTime) : startOfDayMinutes,

                EndTimeMinutes = AsMinutes(endDateTime) <= endOfDayMinutes
                    ? AsMinutes(endDateTime) : endOfDayMinutes
            };
        }

        /// <summary>
        ///  take datetime Hours and add minutes to give total minutes.
        /// </summary>
        private int AsMinutes(DateTime d)
        {
            return (d.Hour * 60) + d.Minute;
        }

        /// <summary>
        /// exists to reduce size of calling method-downside alot of parameters. would refactor further
        /// </summary>
        private double CalculateMultipleDaysTotalHours(DateTime startDateTime, DateTime endDateTime, int businessDays, MinutesTimeSpan timeMinutes)
        {
            var chargeableHoursInDay = this.endOfDayHour - this.startOfDayHour; // aka 10 hours per day in original spec

            // 2 or more days
            var minutesInFirstDay = 0.0;
            if (startDateTime.IsWeekDay()
                && timeMinutes.StartTimeMinutes <= (this.endOfDayHour * 60))
            {
                // calculate hours for first day
                minutesInFirstDay = (this.endOfDayHour * 60) - timeMinutes.StartTimeMinutes;
                businessDays--; //adjust
            }

            var minutesInlastDay = 0.0;
            if (endDateTime.IsWeekDay()
                && timeMinutes.EndTimeMinutes >= (this.startOfDayHour * 60))
            {
                // calculate hours for last days
                minutesInlastDay = timeMinutes.EndTimeMinutes - (this.startOfDayHour * 60);
                businessDays--;//adjust
            }

            var totalFirstAndLastDaysMinutes = minutesInlastDay + minutesInFirstDay;

            var hours = ((businessDays * chargeableHoursInDay * 60.0)
                + totalFirstAndLastDaysMinutes) / 60.0;

            return hours;
        }
    }
}