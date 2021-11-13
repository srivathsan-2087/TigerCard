using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test
{
    public class FareAggregatorTests
    {
        private IFareAggregator aggregator;

        public FareAggregatorTests()
        {
            var fareCalculatorMock = new Mock<IFareCalculator>();
            var fareSettingsMock = new Mock<IFareSettings>();

            fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 1m));

            var fareCapLimits = new Dictionary<FareCapType, decimal>
            {
                {FareCapType.Daily, 5 },
                {FareCapType.Weekly, 15 },
            };

            fareSettingsMock
                .Setup(m => m.GetFareCap(It.IsAny<Zones>(), It.IsAny<Zones>()))
                .Returns<Zones, Zones>((o, d) => new FareCap(o, d, fareCapLimits));

            this.aggregator = new FareAggregator(fareSettingsMock.Object, fareCalculatorMock.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void AggregateTotalFare_WhenJourneysAreNullOrEmpty_ShouldReturnZero(bool isNull)
        {
            // Actors
            Journey[] journeys = isNull ? null : Array.Empty<Journey>();

            // Activity
            var aggregatedFare = this.aggregator.AggregateTotalFare(journeys);

            // Asserts
            Assert.Equal(0m, aggregatedFare);
        }

        [Fact]
        public void AggregateTotalFare_WhenJourneysAreNotNull_ShouldReturnAggregatedFare()
        {
            // Actors
            const decimal ExpectedAggregateFare = 21m;
            var journeys = Enumerable.Range(1, 3)
                                     .SelectMany(i => Enumerable.Range(1, 7).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(j%7), new TimeSpan())))
                                     .ToList();

            // Activity
            var aggregatedFare = this.aggregator.AggregateTotalFare(journeys);

            // Assert
            Assert.Equal(ExpectedAggregateFare, aggregatedFare);
        }
    }
}
