using System;
using System.Linq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Unity;

using Xunit;

namespace TigerCard.IntegrationTest
{
    public class SingleDayPeakHourJourneysTests
    {
        private IFareComputationService service;

        public SingleDayPeakHourJourneysTests()
        {
            var container = new UnityContainer();
            UnityRegistrar.RegisterTypes(container);
            this.service = container.Resolve<IFareComputationService>();
        }

        [Fact]
        public void FareComputation_IntraZoneTripsNotExceedingDailyCap()
        {
            // Actor
            var journeys = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(18, 45, 0)));

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(90, totalFare);
        }

        [Fact]
        public void FareComputation_IntraZoneTripsExceedingDailyCap()
        {
            // Actor
            var journeys = Enumerable.Range(1, 7).Select(i => new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(10, 15, 0)));

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(100, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsNotExceedingDailyCap()
        {
            // Actor
            var journeys = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone1 : Zones.Zone2, DayOfWeek.Thursday, new TimeSpan(10, 15, 0)));

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(100, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsExceedingDailyCap()
        {
            // Actor
            var journeys = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone1 : Zones.Zone2, DayOfWeek.Thursday, new TimeSpan(18, 45, 0)));

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(120, totalFare);
        }
    }
}
