using System;
using Xunit;

namespace Parking.Library.Test
{
    public class LongStayTicketTest : BaseUnitTest
    {
        const float dailyRate = 7.50F;

        [Fact]
        public void GivenValidLongStayTicketFor3DaysFirstAndLastDaysAreOutsideChargeableTimes_WhenCalculateChargeCalled_ThenReturnsChargeAs22_50()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 18:01:00");
            var toDateTime = DateTimeParse("09/09/2020 07:59:00");
            var longTicket = new LongStayTicket(fromDateTime, Constants.LongStayPrice);

            var expectedCharge = 1 * dailyRate; 

            // act
            var charge = longTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
        }

        [Fact]
        public void GivenValidLongStayTicketFor3Days_WhenCalculateChargeCalled_ThenReturnsChargeAs22_50()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 07:50:00");
            var toDateTime = DateTimeParse("09/09/2020 05:20:00");
            var longTicket = new LongStayTicket(fromDateTime, Constants.LongStayPrice);

            var expectedCharge = 2 * dailyRate; // 2 * 7.5 = 15.0, since last day is before tne start of business

            // act
            var charge = longTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
        }

        [Fact]
        public void GivenValidLongStayTicketInsideOfHours_WhenCalculateChargeCalled_ThenReturnChargeFor1DayAs7_50()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 15:50:00");
            var toDateTime = DateTimeParse("07/09/2020 20:20:00");
            var longTicket = new LongStayTicket(fromDateTime, Constants.LongStayPrice);

            var expectedCharge = 1 * dailyRate;

            // act
            var charge = longTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
        }

        [Fact]
        public void GivenValidLongStayTicketFor2Days_WhenCalculateCharge_ThenReturnChargeAs15_00()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 12:50:00");
            var toDateTime = DateTimeParse("08/09/2020 07:59:00");
            var longTicket = new LongStayTicket(fromDateTime, Constants.LongStayPrice);

            // act
            var charge = longTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(7.5), Round(charge.Value));
        }

        [Fact]
        public void GivenInvalidLongStayTicket_WhenCalculateCharge_ThenShouldEqualExpected()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 07:50:00");
            var toDateTime = DateTimeParse("06/09/2020 05:20:00");
            var longTicket = new LongStayTicket(fromDateTime, Constants.LongStayPrice);

            // act + assert
            Assert.Throws<ArgumentException>(() =>
                {
                    _ = longTicket.CalculateCharge(toDateTime);
                }
            );
        }

    }
}
