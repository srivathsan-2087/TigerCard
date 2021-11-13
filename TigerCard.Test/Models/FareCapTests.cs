using System;
using System.Collections.Generic;

using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test.Models
{
    public class FareCapTests
    {
        [Fact]
        public void Constructor_WhenCalledWithNullLimits_ShouldThrowArgumentException()
        {
            // Actors

            // Activity
            var exception = Assert.Throws<ArgumentException>(() => new FareCap(Zones.Zone1, Zones.Zone2, null));

            // Asserts
            Assert.Equal("inputLimits cannot be null (Parameter 'inputLimits')", exception.Message);
            Assert.Equal("inputLimits", exception.ParamName);
        }

        [Fact]
        public void Constructor_WhenCalledWithLimits_ShouldCopyLimitsToPrivateDictionary()
        {
            // Actors
            var dictionary = new Dictionary<FareCapType, decimal>
            {
                { FareCapType.Daily, 13 },
                { FareCapType.Weekly, 14 }
            };

            // Activity
            var fareCap = new FareCap(Zones.Zone1, Zones.Zone1, dictionary);

            // Assert
            Assert.Equal(13, fareCap.GetFareCap(FareCapType.Daily));
            Assert.Equal(14, fareCap.GetFareCap(FareCapType.Weekly));
        }
    }
}
