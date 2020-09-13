using System;

namespace Parking.Library
{
    /// <summary>
    /// an implementation of the Ticket should have a price and qty/unit calculation method aka DurationUnits
    /// </summary>
    public abstract class Ticket 
    {
        public Ticket(DateTime startDateTime, float pricePerUnit)
        {
            this.StartTime = startDateTime;
            this.UnitPrice = pricePerUnit; // hour or day etc
        }

        public DateTime StartTime { get; private set; }
        
        public DateTime? EndTime {get; set;}

        public float UnitPrice { get; private set; }

        public abstract double DurationUnits();

        public Charge CalculateCharge(DateTime? endTime = null)
        {
            this.EndTime = endTime ?? this.EndTime;

            ValidateTicketDates();

            var unitsToCharge = this.DurationUnits();
            var chargeValue = unitsToCharge * this.UnitPrice;

            return new Charge(chargeValue);
        }

        private void ValidateTicketDates()
        {
            if (!this.EndTime.HasValue)
            {
                throw new InvalidOperationException("Cannot calculate charge without end date");
            }

            if (this.EndTime < this.StartTime)
            {
                throw new ArgumentException("End date cannot be before the start date");
            }
        }
    }
}