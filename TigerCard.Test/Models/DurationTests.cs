using System;

using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test.Models
{
    public class DurationTests
    {
        [Fact]
        public void Constructor_WhenCalledWithStartTimeAndInterval_ShouldSetEndTime()
        {
            // Actor
            var start = new TimeSpan(10, 0, 0);

            // Activity
            var duration = new Duration(start, 180);

            // Asserts
            Assert.Equal(new TimeSpan(13, 0, 0), duration.End);
        }

        [Fact]
        public void Constructor_WhenCalledWithStartTimeAndNegativeInterval_ShouldDiscardNegativeSignAndSetEndTime()
        {
            // Actor
            var start = new TimeSpan(10, 0, 0);

            // Activity
            var duration = new Duration(start, -180);

            // Asserts
            Assert.Equal(new TimeSpan(7, 0, 0), duration.End);
        }

        [Fact]
        public void Constructor_WhenCalledWithStartAndEndTime_ShouldSetIntervalInMinutes()
        {
            // Actors
            var start = new TimeSpan(10, 11, 0);
            var end = new TimeSpan(12, 43, 0);

            // Activity
            var duration = new Duration(start, end);

            // Asserts
            Assert.Equal(152, duration.Interval);
        }
    }
}
