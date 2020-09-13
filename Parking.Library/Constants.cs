using System;

namespace Parking.Library
{
    public static class Constants // could replace with a ConstantsProvider, or get from AppSettings
    {
        public const float LongStayPrice = 7.50F; 
        public const float ShortStayPrice = 1.10F;

        public const int BusinessHoursStart = 8;
        public const int BusinessHoursEnd = 18;

        public static DayOfWeek[] WeekendDays = new DayOfWeek[] { DayOfWeek.Saturday, DayOfWeek.Sunday };
    }
}
