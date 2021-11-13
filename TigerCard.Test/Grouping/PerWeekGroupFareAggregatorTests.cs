using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using TigerCard.Business.Grouping;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test.Grouping
{
    public class PerWeekGroupFareAggregatorTests
    {
        private IJourneyGroupFareAggregator aggregator;

        private Mock<IFareCalculator> fareCalculatorMock;
        private Mock<IFareSettings> fareSettingsMock;

        public PerWeekGroupFareAggregatorTests()
        {
            this.fareCalculatorMock = new Mock<IFareCalculator>();
            this.fareSettingsMock = new Mock<IFareSettings>();

            var fareCapLimits = new Dictionary<FareCapType, decimal>
            {
                {FareCapType.Daily, 15 },
                {FareCapType.Weekly, 75 },
            };

            this.fareSettingsMock
                .Setup(m => m.GetFareCap(It.IsAny<Zones>(), It.IsAny<Zones>()))
                .Returns<Zones, Zones>((o, d) => new FareCap(o, d, fareCapLimits));

            this.aggregator = new PerWeekGroupFareAggregator(this.fareSettingsMock.Object, this.fareCalculatorMock.Object);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ComputeJourneyFare_WhenJourneysAreEmptyOrNull_ShouldReturnEmptyGroup(bool isNull)
        {
            // Actors
            Journey[] journeys = isNull ? null : Array.Empty<Journey>();

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.Empty(journeyGroups);
        }

        [Fact]
        public void ComputeJourneyFare_WhenJourneysBelongToSameWeek_ShouldReturnSingleGroup()
        {
            // Actors
            var journeys = Enumerable.Range(1, 7) // For each day of week set up 2 journey
                                     .SelectMany(i => Enumerable.Range(1, 2).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 1m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.Single(journeyGroups);
        }

        [Fact]
        public void ComputeJourneyFare_WhenJourneysForMoreThanOneWeek_ShouldReturnMultipleGroups()
        {
            // Actors
            var journeys = Enumerable.Range(1, 2) // Set up 2 weeks with 1 journey per day of week
                                     .SelectMany(i => Enumerable.Range(1, 7).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(j % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 1m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.Equal(2, journeyGroups.Count());
        }

        [Fact]
        public void ComputeJourneyFare_WhenWeeklyCapIsNotReached_ShouldSetJourneyFareCapTypeAsNone()
        {
            // Actors
            var journeys = Enumerable.Range(1, 7) // For each day of week set up 7 journeys
                                     .SelectMany(i => Enumerable.Range(1, 7).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns(() => new JourneyFare(true, 1m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys);

            // Assert
            Assert.True(journeys.All(j => j.Fare.Price == 1m));
        }

        [Fact]
        public void ComputeJourneyFare_WhenWeeklyCapIsReached_ShouldSetJourneyFareCapTypeAsWeekly()
        {
            // Actors
            var journeys = Enumerable.Range(1, 7) //For each day of week setup 5 journeys
                                     .SelectMany(i => Enumerable.Range(1, 5).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns<Zones, Zones, DayOfWeek, TimeSpan>((o, d, day, t) => new JourneyFare(true, 7m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            // Assert
            var weekendJourneys = journeyGroups.Single().Journeys.GroupBy(j => j.Day)
                                               .Where(grp => grp.Key == DayOfWeek.Saturday && grp.Key == DayOfWeek.Sunday) // Take all journeys of weekends
                                               .SelectMany(grp => grp).ToArray();

            Assert.True(weekendJourneys.All(j => j.Fare.AppliedCapType == FareCapType.Weekly && j.Fare.Price == 0m));
        }

        [Fact]
        public void ComputeJourneyFare_WhenDailyCapIsReachedAndWeeklyCapIsNotReached_ShouldSetJourneyFareCapTypeAsDaily()
        {
            // Actors
            var journeys = Enumerable.Range(1, 7) //For each day of week setup 5 journeys
                                     .SelectMany(i => Enumerable.Range(1, 5).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns<Zones, Zones, DayOfWeek, TimeSpan>((o, d, day, t) => new JourneyFare(true, 7m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            // Assert
            var weekDayJourneys = journeyGroups.Single().Journeys.GroupBy(j => j.Day)
                                               .Where(grp => grp.Key != DayOfWeek.Saturday && grp.Key != DayOfWeek.Sunday) // Filter out all journeys for weekend
                                               .ToDictionary(grp => grp.Key, grp => grp.ToArray());

            var dailyCappedJourneys = weekDayJourneys.Values.SelectMany(arr => arr.Where((journey, index) => index > 1));
            Assert.True(dailyCappedJourneys.Take(13).All(journey => journey.Fare.AppliedCapType == FareCapType.Daily));
            Assert.True(dailyCappedJourneys.TakeLast(2).All(journey => journey.Fare.AppliedCapType == FareCapType.Weekly)); // After 3rd journey on Friday, Weekly cap would have been reached
        }

        [Fact]
        public void ComputeJourneyFare_WhenDailyCapAndWeeklyCapAreNotReached_ShouldSetJourneyFareCapTypeAsNone()
        {
            // Actors
            var journeys = Enumerable.Range(1, 7) //For each day of week setup 5 journeys
                                     .SelectMany(i => Enumerable.Range(1, 5).Select(j => new Journey(Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan())))
                                     .ToArray();

            this.fareCalculatorMock
                .Setup(m => m.GetJourneyFare(It.IsAny<Zones>(), It.IsAny<Zones>(), It.IsAny<DayOfWeek>(), It.IsAny<TimeSpan>()))
                .Returns<Zones, Zones, DayOfWeek, TimeSpan>((o, d, day, t) => new JourneyFare(true, 7m));

            // Activity
            var journeyGroups = this.aggregator.ComputeJourneyFare(journeys).ToArray();

            // Assert
            var weekDayJourneys = journeyGroups.Single().Journeys.GroupBy(j => j.Day)
                                               .Where(grp => grp.Key != DayOfWeek.Saturday && grp.Key != DayOfWeek.Sunday) // Filter out all journeys for weekend
                                               .ToDictionary(grp => grp.Key, grp => grp.ToArray());

            var uncappedDailyJourneys = weekDayJourneys.Values.SelectMany(arr => arr.Where((journey, index) => index <= 1));
            Assert.True(uncappedDailyJourneys.All(journey => journey.Fare.AppliedCapType == FareCapType.None));
        }
    }
}
