using System;
using Xunit;

namespace Parking.Library.Test
{
    public class ShortStayTicketTest : BaseUnitTest
    {
        const float hourlyRate = 1.1F;

        [Fact]
        public void TestShortTicketFromSample33()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2017 16:50:00");
            var toDateTime = DateTimeParse("09/09/2017 19:15:00");
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, new HourlyCalculator(8, 18));

            // act
            var charge = shortTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(12.28F), Round(charge.Value));
        }

        [Fact]
        public void Test1ShortTicketFromSample33()
        {
            // arrange
            var fromDateTime = DateTimeParse("09/09/2017 06:50:00");
            var toDateTime = DateTimeParse("09/09/2017 19:15:00");
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, new HourlyCalculator(8, 18));
            
            var expectedCharge = 0.0; //none it's a weekend

            //act
            var charge = shortTicket.CalculateCharge(toDateTime);

//
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
        }

        [Fact]
        public void Test2ShortTicketFromSample33()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 16:50:00");
            var toDateTime = DateTimeParse("09/09/2020 19:15:00");
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, new HourlyCalculator(8, 18));

            var expectedCharge = 21.16 * hourlyRate;  // 1hr 1m + 20hrs(2days) = 21h 10m or 11.16 decimal

            // act
            var charge = shortTicket.CalculateCharge(toDateTime);

            // asert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
        }
    }
}