using System;

namespace Parking.Library
{
    public interface IDurationCalculator
    {
        double GetTotalChargeableQuantity(DateTime startDateTime, DateTime endDateTime);
    }
}
