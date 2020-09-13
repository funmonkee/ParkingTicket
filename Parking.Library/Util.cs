using System;

namespace Parking.Library
{
    public static class Util
    {
        /// <summary>
        /// Get days between 2 dates. Add 1 to round up.
        /// </summary>
        public static double GetDays(DateTime startDate, DateTime endDate)
        {
            // throw exception if end is before start
            if (endDate < startDate)
                throw new ArgumentException("end cannot be less than start");

            return (endDate.Date - startDate.Date).TotalDays + 1;
        }
   }
}