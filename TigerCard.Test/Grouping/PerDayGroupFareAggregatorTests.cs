using System;
using System.Linq;

using Moq;

using TigerCard.Business.Grouping;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;
using TigerCard.Business.Settings;

using Xunit;

namespace TigerCard.Test.Grouping
{
    public class PerDayGroupFareAggregatorTests
    {
        private IJourneyGroupFareAggregator aggregator;
        private Mock<IFareCalculator> fareCalculatorMock;

        public PerDayGroupFareAggregatorTests()
        {
            this.fareCalculatorMock = new Mock<IFareCalculator>();
            var fareSettings = FareSettingsFactory.CreateDefaultSettings();
            this.aggregator = new PerDayGroupFareAggregator(fareSettings, this.fareCalculatorMock.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ComputeJourneyFare_WhenJourneysAreNullOrEmpty_ShouldReturnEmptyJourneyGroup(bool isNull)
        {
            // Actors
            Journey[] journeys = isNull ? null : new Journey[0];

            // Activity
            var journeyGroup = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.Empty(journeyGroup);

        }

        [Fact]
        public void ComputeJourneyFare_WhenJourneysAreForSameDayOfWeek_ShouldReturnSingleJourneyGroup()
        {
            // Actors
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey2 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey3 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey4 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());

            var journeys = new[] { journey1, journey2, journey3, journey4 };
            
            var journeyFare = new JourneyFare(true, 5m);

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(journeyFare);

            // Activity
            var journeyGroup = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.Single(journeyGroup);
        }

        [Fact]
        public void ComputeJourneyFare_WhenJourneysAreBetweenDifferentZones_ShouldApplyCapBasedOnFarthestJourney()
        {
            // Actors
            decimal ExpectedCappedFare = 120m;
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey2 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());
            var journey3 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey4 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());
            var journey5 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey6 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());

            var journeys = new[] { journey1, journey2, journey3, journey4, journey5, journey6 };

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 55m));

            // Activity
            var journeyGroup = this.aggregator.ComputeJourneyFare(journeys);
            var cappedFare = journeyGroup.Sum(grp => grp.Journeys.Sum(j => j.Fare.Price));

            // Assert
            Assert.Equal(ExpectedCappedFare, cappedFare);
        }

        [Fact]
        public void ComputeJourneyFare_WhenJourneysAreBetweenDifferentZonesOnDifferentDays_ShouldApplyCapBasedOnFarthestJourneyOnEachDay()
        {
            // Actors
            decimal ExpectedCappedFare = 120m;
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey2 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());
            var journey3 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey4 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());
            var journey5 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Tuesday, new TimeSpan());
            var journey6 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());

            var journeys = new[] { journey1, journey2, journey3, journey4, journey5, journey6 };

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 65m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            var cappedFareForMonday = journeyGroups[0].Journeys.Sum(j => j.Fare.Price);
            var cappedFareForTuesday = journeyGroups[1].Journeys.Sum(j => j.Fare.Price);

            // Assert
            Assert.Equal(ExpectedCappedFare, cappedFareForMonday);
            Assert.Equal(ExpectedCappedFare, cappedFareForTuesday);
        }

        [Fact]
        public void ComputeJourneyFare_WhenCapIsReachedForADayOfWeek_ShouldLimitPriceOfJourneys()
        {
            // Actors
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey2 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());
            var journey3 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey4 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());
            var journey5 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Tuesday, new TimeSpan());
            var journey6 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());

            var journeys = new[] { journey1, journey2, journey3, journey4, journey5, journey6 };

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 65m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            // Assert
            Assert.Equal(55m, journey2.Fare.Price);
            Assert.Equal(0m, journey3.Fare.Price);
            Assert.Equal(55m, journey5.Fare.Price);
            Assert.Equal(0m, journey6.Fare.Price);
        }

        [Fact]
        public void ComputeJourneyFare_WhenCapIsReachedForADayOfWeek_ShouldSetCapTypeOfJourneyToDaily()
        {
            // Actors
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey2 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan());
            var journey3 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan());
            var journey4 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());
            var journey5 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Tuesday, new TimeSpan());
            var journey6 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan());

            var journeys = new[] { journey1, journey2, journey3, journey4, journey5, journey6 };

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 65m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            // Assert
            Assert.Equal(FareCapType.None, journey1.Fare.AppliedCapType);
            Assert.Equal(FareCapType.Daily, journey2.Fare.AppliedCapType);
            Assert.Equal(FareCapType.Daily, journey3.Fare.AppliedCapType);
            Assert.Equal(FareCapType.None, journey4.Fare.AppliedCapType);
            Assert.Equal(FareCapType.Daily, journey5.Fare.AppliedCapType);
            Assert.Equal(FareCapType.Daily, journey6.Fare.AppliedCapType);
        }
    }
}
