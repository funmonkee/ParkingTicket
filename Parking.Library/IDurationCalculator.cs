using System;

namespace Parking.Library
{
    public interface IDurationCalculator
    {
        double GetTotalChargeableQuantity(DateTime startDateTime, DateTime endDateTime);
    }
    public class HourlyCalculator : IDurationCalculator
    {
        public HourlyCalculator(int startOfDayHour, int endOfDayHour)
        {
            this.startOfDayHour = startOfDayHour;
            this.endOfDayHour = endOfDayHour;
        }

        /// <summary>
        /// given a start and end time. determine business hours between
        /// first get business days and then trim to determine partial hours
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        public double GetTotalChargeableQuantity(DateTime startDateTime, DateTime endDateTime)
        {
            // throw exception if end is before start
            if (endDateTime < startDateTime)
                throw new ArgumentException("end cannot be less than start");

            var businessDays = GetBusinessDays(startDateTime, endDateTime);
            if (businessDays == 0) return 0;

            var timeAsMinutes = BuildBoundedMinutesTimeSpan(startDateTime, endDateTime);

            // cut off
            if (businessDays == 1) // just one day, same day in and out, calculate the hours
            {
                var dayHours = (double)Math.Ceiling(
                    (decimal)(timeAsMinutes.EndTimeMinutes - timeAsMinutes.StartTimeMinutes)
                    / 60);
                return dayHours > 0 ? dayHours : 1; // minimum one hour charge
            }

            var hours = CalculateHours(startDateTime, endDateTime, businessDays, timeAsMinutes);
            return hours;
        }

        /// <summary>
        /// this is the bsuiness days. days that do not imclude sat and sunday
        /// see here https://alecpojidaev.wordpress.com/2009/10/29/work-days-calculation-with-c/
        /// </summary>
        /// <param name="startD"></param>
        /// <param name="endD"></param>
        /// <returns></returns>
        public int GetBusinessDays(DateTime startD, DateTime endD)
        {
            ///throw exception if end is before start
            // discards https://docs.microsoft.com/en-us/dotnet/csharp/discards
            ///_ = startD ?? throw new ArgumentNullException(nameof(startD));
            // http://www.codinghelmet.com/articles/why-do-we-need-guard-clauses
            if (endD < startD)
            {
                throw new ArgumentException("end cannot be less than start");
            }

            var workDaysMultiplier = (endD - startD).TotalDays * 5; //35
            var weekendDaysMultiplier = (startD.DayOfWeek - endD.DayOfWeek) * 2; // 14  . 35-14 =21

            double workDays = 1 + ((workDaysMultiplier - weekendDaysMultiplier) / 7);  // 3+1=4

            if (endD.DayOfWeek == DayOfWeek.Saturday) workDays--; // adjust
            if (startD.DayOfWeek == DayOfWeek.Sunday) workDays--; // adjust

            return (int)Math.Round(workDays);
        }

        // move to the short stay ticket

        private readonly int startOfDayHour;
        private readonly int endOfDayHour;

        /// <summary>
        /// a timespan that does not pass the business day bounderies. this is for calculation only
        /// </summary>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <returns></returns>
        private MinutesTimeSpan BuildBoundedMinutesTimeSpan(DateTime startDateTime, DateTime endDateTime)
        {
            var startOfDayMinutes = this.startOfDayHour * 60; // 8am
            var endOfDayMinutes = this.endOfDayHour * 60; // 6pm

            return new MinutesTimeSpan
            {
                StartTimeMinutes = AsMinutes(startDateTime) >= startOfDayMinutes
                    ? AsMinutes(startDateTime) : startOfDayMinutes,

                EndTimeMinutes = AsMinutes(endDateTime) <= endOfDayMinutes
                    ? AsMinutes(endDateTime) : endOfDayMinutes
            };
        }

        private int AsMinutes(DateTime d)
        {
            return (d.Hour * 60) + d.Minute;
        }

        // exists to reduce size of calling method-downside alot of parameters. would refactor further
        private double CalculateHours(
            DateTime startDateTime,
            DateTime endDateTime,
            int businessDays,
            MinutesTimeSpan timeMinutes
            )
        {
            var chargeableHoursInDay = this.endOfDayHour - this.startOfDayHour; //10

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
