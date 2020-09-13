using System;
using Xunit;

namespace Parking.Library.Test
{
    public class HourCalculatorTest : BaseUnitTest
    {
        [Fact]
        public void GivenValidDateRangeWith7Days_WhenGetDaysCalled_ThenReturns7Days()
        {
            var fromDateTime = DateTimeParse("07/09/2017 16:50:00");
            var toDateTime = DateTimeParse("15/09/2017 19:15:00");
            var calc = new HourlyCalculator(8, 18);

            var days = calc.GetBusinessDays(fromDateTime, toDateTime);

            Assert.Equal(7, days);
        }

        [Fact]
        public void GivenFromToDates_WhenGetTotalChargeableHoursCalled_ThenReturns61Hours17Mins()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 16:50:00");
            var toDateTime = DateTimeParse("15/09/2020 19:15:00");
            var calc = new HourlyCalculator(8, 18);

            // act
            var hours = calc.GetTotalChargeableQuantity(fromDateTime, toDateTime);

            // assert
            Assert.Equal(61.17, Round(hours)); //5*10 = 50, 2 10
        }

        [Theory]
        [InlineData("01/09/2020 12:00:00", "01/09/2020 12:10:00", 1)]
        [InlineData("01/09/2020 12:00:00", "02/09/2020 12:10:00", 10.17)]
        [InlineData("15/09/2020 12:00:00", "17/09/2020 11:59:00", 19.98)] // 16th=11, 6+4 =10
        [InlineData("11/09/2020 09:00:00", "11/09/2020 12:00:00", 3)]
        [InlineData("10/09/2020 10:30:00", "11/09/2020 09:30:00", 9)]
        [InlineData("10/09/2020 10:30:00", "11/09/2020 09:00:00", 8.5)] // 7.5 + 1 = 9
        public void GivenFromToDates_WhenGetTotalChargeableHoursCalled_ThenReturnsExpectedHours(string from, string to, double expectedHours)
        {
            // arrange
            var dateFrom = DateTimeParse(from);
            var dateTo = DateTimeParse(to);
            var calc = new HourlyCalculator(8, 18);
// act
            var hours = calc.GetTotalChargeableQuantity(dateFrom, dateTo);
            
            // assert
            Assert.Equal(Round(expectedHours), Round(hours));
        }
    }
}