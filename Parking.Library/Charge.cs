namespace Parking.Library
{
    /// <summary>
    /// model the charge value
    /// </summary>
    public class Charge 
    {
        public Charge(double chargeValue)
        {
            this.Value = chargeValue;
        }
        
        public double Value { get; private set; }
    }
}
