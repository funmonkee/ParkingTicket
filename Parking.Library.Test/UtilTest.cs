using Xunit;

namespace Parking.Library.Test
{
    public class UtilTest :BaseUnitTest
    {
        [Fact]
        public void GivenValidDateRangeWith3Days_WhenGetDaysCalled_ThenReturns3Days()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2017 16:50:00");
            var toDateTime = DateTimeParse("09/09/2017 19:15:00");

            // act
            var days = Util.GetDays(fromDateTime, toDateTime);

            // assert
            Assert.Equal(3, days);
        }

        [Fact]
        public void GivenValidDateRange_WhenCallGetDays_ThenReturnExpectedResult()
        {
            // arrange
            var fromDateTime = DateTimeParse("07/09/2020 07:50:00");
            var toDateTime = DateTimeParse("13/09/2020 05:20:00");

            // act
            var days = Util.GetDays(fromDateTime, toDateTime);

            // assert
            Assert.Equal(7, days);
        }
    }
}