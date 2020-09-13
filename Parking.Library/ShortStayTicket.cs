using System;

namespace Parking.Library
{
    public class ShortStayTicket : Ticket 
    {
        public ShortStayTicket(DateTime startDateTime, float pricePerHour, IDurationCalculator calculator) : base(startDateTime, pricePerHour)
        {
            Calculator = calculator;
        }

        public IDurationCalculator Calculator { get; }

        public override double DurationUnits() 
        { 
            return this.Calculator.GetTotalChargeableQuantity(this.StartTime, this.EndTime.Value); 
        }
    }
}