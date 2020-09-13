using System;

namespace Parking.Library
{
    /// <summary>
    /// a long stay ticket computes the DurationUnits based on number of days between start and end of stay, including weekends
    /// </summary>
    public class LongStayTicket : Ticket 
    {
        public LongStayTicket(DateTime startDateTime, float pricePerDay) : base(startDateTime, pricePerDay)
        {
        }

        public override double DurationUnits() { return Util.GetDays(this.StartTime, this.EndTime.Value ); }
    }
}