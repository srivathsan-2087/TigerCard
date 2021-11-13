using System;
using System.Linq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Unity;

using Xunit;

namespace TigerCard.IntegrationTest
{
    public class SingleWeekPeakHourJourneysTests
    {
        private IFareComputationService service;

        public SingleWeekPeakHourJourneysTests()
        {
            var container = new UnityContainer();
            UnityRegistrar.RegisterTypes(container);
            this.service = container.Resolve<IFareComputationService>();
        }

        [Fact]
        public void FareComputation_IntraZoneTripsNotExceedingWeeklyCap()
        {
            // Actor
            var journeys = Enumerable.Range(5, 3)
                                     .SelectMany(i => Enumerable.Range(1, 3).Select(j => new Journey(Zones.Zone2, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan(19, 45, 0))))
                                     .ToArray();

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(225, totalFare);
        }

        [Fact]
        public void FareComputation_IntraZoneTripsExceedingWeeklyCap()
        {
            // Actor
            var journeys = Enumerable.Range(3, 5)
                                     .SelectMany(i => Enumerable.Range(1, 4).Select(j => new Journey(Zones.Zone2, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan(10, 0, 0))))
                                     .ToArray();

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(400, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsNotExceedingWeeklyCap()
        {
            // Actor
            var journeys = Enumerable.Range(5, 3)
                                     .SelectMany(i => Enumerable.Range(1, 3).Select(j => new Journey((j % 2 == 0) ? Zones.Zone2 : Zones.Zone1, Zones.Zone2, (DayOfWeek)(i % 7), new TimeSpan(19, 45, 0))))
                                     .ToArray();

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(285, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsExceedingWeeklyCap()
        {
            // Actor
            var journeys = Enumerable.Range(3, 5)
                                     .SelectMany(i => Enumerable.Range(1, 4).Select(j => new Journey(Zones.Zone2, (j % 2 == 0) ? Zones.Zone2 : Zones.Zone1, (DayOfWeek)(i % 7), new TimeSpan(10, 0, 0))))
                                     .ToArray();

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(600, totalFare);
        }
    }
}
