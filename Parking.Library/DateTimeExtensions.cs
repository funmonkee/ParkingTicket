using System;
using System.Linq;

namespace Parking.Library
{
    public static class DateTimeExtensions
    {
        public static bool IsWeekDay(this DateTime dateTime)
        {
            return !Constants.WeekendDays.Contains(dateTime.DayOfWeek);;
        }
    }
}
