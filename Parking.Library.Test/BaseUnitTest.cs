using System;
using System.Globalization;

namespace Parking.Library.Test
{
    public class BaseUnitTest
    {
        protected double Round(double value)
        {
            return Math.Round( value * 100f) / 100f ;
        }

        protected DateTime DateTimeParse(string stringDateTime)
        {
            var dateTime = DateTime.ParseExact(
                stringDateTime, 
                "dd/MM/yyyy HH:mm:ss", 
                CultureInfo.CurrentCulture);
            return dateTime; 
        }
    }
}
