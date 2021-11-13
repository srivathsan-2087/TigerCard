using System;

using TigerCard.Business.Extensions;
using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test.Extensions
{
    public class TimeSpanExtensionTests
    {
        [Fact]
        public void IsBetween_WhenTimeSpanIsEarlierThanDuration_ShouldReturnFalse()
        {
            // Actors
            var timeSpan = new TimeSpan(6, 59, 59);

            // Assert
            Assert.False(timeSpan.IsBetween(new Duration(new TimeSpan(7, 0, 0), 120)));
        }

        [Fact]
        public void IsBetween_WhenTimeSpanIsStartOfDuration_ShouldReturnTrue()
        {
            // Actors
            var timeSpan = new TimeSpan(7, 0, 0);

            // Assert
            Assert.True(timeSpan.IsBetween(new Duration(new TimeSpan(7, 0, 0), 120)));
        }

        [Fact]
        public void IsBetween_WhenTimeSpanIsBetweenDuration_ShouldReturnTrue()
        {
            // Actors
            var timeSpan = new TimeSpan(8, 0, 0);

            // Assert
            Assert.True(timeSpan.IsBetween(new Duration(new TimeSpan(7, 0, 0), 120)));
        }

        [Fact]
        public void IsBetween_WhenTimeSpanIsEndOfDuration_ShouldReturnTrue()
        {
            // Actors
            var timeSpan = new TimeSpan(9, 0, 0);

            // Assert
            Assert.True(timeSpan.IsBetween(new Duration(new TimeSpan(7, 0, 0), 120)));
        }

        [Fact]
        public void IsBetween_WhenTimeSpanIsLaterThanDuration_ShouldReturnFalse()
        {
            // Actors
            var timeSpan = new TimeSpan(9, 0, 1);

            // Assert
            Assert.False(timeSpan.IsBetween(new Duration(new TimeSpan(7, 0, 0), 120)));
        }
    }
}
