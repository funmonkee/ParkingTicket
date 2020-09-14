using System;
using Xunit;
using Moq;

namespace Parking.Library.Test
{
    public class ShortStayTicketTest : BaseUnitTest
    {
        const float hourlyRate = 1.1F;

        Mock<IDurationCalculator> mockDurationCalc = null; 

        public ShortStayTicketTest()
        {
            this.mockDurationCalc = new Mock<IDurationCalculator>();
        }

        [Fact]
        public void GivenValidDates1Day1Hour10Mins_When_Then()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2017 16:50:00");
            var toDateTime = DateTimeParse("09/09/2017 19:15:00");

            this.mockDurationCalc
                .Setup(m => m.GetTotalChargeableQuantity(It.Is<DateTime>(d => d == fromDateTime), It.Is<DateTime>(d => d == toDateTime)))
                .Returns(11.16d);
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, this.mockDurationCalc.Object);

            var expectedChange = 12.28F;

            // act
            var charge = shortTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(expectedChange), Round(charge.Value));
        }

        [Fact]
        public void GivenWeekendDateStay_WhenCalculatedCalled_ThenReturnNoCharge()
        {
            // arrange
            var fromDateTime = DateTimeParse("09/09/2017 06:50:00");
            var toDateTime = DateTimeParse("09/09/2017 19:15:00");

            this.mockDurationCalc
                .Setup(m => m.GetTotalChargeableQuantity(It.Is<DateTime>(d => d == fromDateTime), It.Is<DateTime>(d => d == toDateTime)))
                .Returns(0.0D);
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, this.mockDurationCalc.Object);
            
            var expectedCharge = 0.0; //none it's a weekend-noo charge

            // act
            var charge = shortTicket.CalculateCharge(toDateTime);

            // assert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
            mockDurationCalc.Verify(m => m.GetTotalChargeableQuantity(It.Is<DateTime>(d => d == fromDateTime), It.Is<DateTime>(d => d == toDateTime)));
        }

        [Fact]
        public void GivenValidStayOf2Days1Hour10Mins_WhenCalculationCalled_ThenReturn23_28()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 16:50:00");
            var toDateTime = DateTimeParse("09/09/2020 19:15:00");

            this.mockDurationCalc
                .Setup(m => m.GetTotalChargeableQuantity(It.Is<DateTime>(d => d == fromDateTime), It.Is<DateTime>(d => d == toDateTime)))
                .Returns(21.16d);
            var shortTicket = new ShortStayTicket(fromDateTime, Constants.ShortStayPrice, this.mockDurationCalc.Object);

            var expectedCharge = 21.16 * hourlyRate;  // 1hr 10m + 20hrs(2days) = 21h 10m or 21.16 double

            // act
            var charge = shortTicket.CalculateCharge(toDateTime);

            // asert
            Assert.Equal(Round(expectedCharge), Round(charge.Value));
            mockDurationCalc.Verify(m => m.GetTotalChargeableQuantity(It.Is<DateTime>(d => d == fromDateTime), It.Is<DateTime>(d => d == toDateTime)));
        }
    }
}