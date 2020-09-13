using System;
using Xunit;

namespace Parking.Library.Test
{
    public class DateTimeExtensionsTest : BaseUnitTest
    {
        [Theory]
        [InlineData("12/09/2020 11:00:00", false)]
        [InlineData("11/09/2020 09:00:00", true)]
        [InlineData("10/09/2020 10:30:00", true)]
        public void IsWeekDayAndHour_When_Then(string dateTime, bool shouldBeTrue)
        {
            // arrange
            var date = DateTimeParse(dateTime);

            // act
            var result = date.IsWeekDay();

            // assert
            Assert.True(result == shouldBeTrue);
        }
    }
}