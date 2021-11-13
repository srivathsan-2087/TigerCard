using System;
using System.Collections.Generic;
using System.Linq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Unity;

using Xunit;

namespace TigerCard.IntegrationTest
{
    public class MultiWeekMixedTimeJourneysTests
    {
        private IFareComputationService service;

        public MultiWeekMixedTimeJourneysTests()
        {
            var container = new UnityContainer();
            UnityRegistrar.RegisterTypes(container);
            this.service = container.Resolve<IFareComputationService>();
        }

        [Fact]
        public void FareComputation_IntraZoneTripsNotExceedingDailyCap()
        {
            // Actor
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey2 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(18, 0, 0));
            var journey3 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Tuesday, new TimeSpan(13, 0, 0));
            var journey4 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Tuesday, new TimeSpan(18, 0, 0));
            var journey5 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(13, 0, 0));
            var journey6 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(13, 0, 0));
            var journey7 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Friday, new TimeSpan(13, 0, 0));
            var journey8 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Friday, new TimeSpan(15, 0, 0));
            var journey9 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(13, 0, 0));
            var journey10 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(18, 0, 0));
            var journey11 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(19, 0, 0));
            var journey12 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey13 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(13, 30, 0));
            var journey14 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(14, 0, 0));

            var journeys = new Journey[] { journey1, journey2, journey3, journey4, journey5, journey6, journey7, journey8, journey9, journey10, journey11, journey12, journey13, journey14 };

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(355, totalFare);
        }

        [Fact]
        public void FareComputation_IntraZoneTripsExceedingDailyCap()
        {
            // Actor
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey2 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(18, 0, 0));
            var journey3 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(18, 20, 0));
            var journey4 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(18, 50, 0));
            var journey5 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(19, 10, 0));
            var journey6 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(19, 35, 0));
            var journey7 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Friday, new TimeSpan(13, 0, 0));
            var journey8 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Friday, new TimeSpan(15, 0, 0));
            var journey9 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(13, 0, 0));
            var journey10 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(18, 0, 0));
            var journey11 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(19, 0, 0));
            var journey12 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey13 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(13, 30, 0));
            var journey14 = new Journey(Zones.Zone2, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(14, 0, 0));

            var journeys = new Journey[] { journey1, journey2, journey3, journey4, journey5, journey6, journey7, journey8, journey9, journey10, journey11, journey12, journey13, journey14 };

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(295, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsNotExceedingDailyCap()
        {// Actor
            var journey1 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey2 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(18, 0, 0));
            var journey3 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan(13, 0, 0));
            var journey4 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Tuesday, new TimeSpan(18, 0, 0));
            var journey5 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(13, 0, 0));
            var journey6 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(13, 0, 0));
            var journey7 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Friday, new TimeSpan(13, 0, 0));
            var journey8 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Friday, new TimeSpan(15, 0, 0));
            var journey9 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Sunday, new TimeSpan(13, 0, 0));
            var journey10 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(18, 0, 0));
            var journey11 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Sunday, new TimeSpan(19, 0, 0));
            var journey12 = new Journey(Zones.Zone2, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(13, 0, 0));
            var journey13 = new Journey(Zones.Zone1, Zones.Zone1, DayOfWeek.Monday, new TimeSpan(13, 30, 0));
            var journey14 = new Journey(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(14, 0, 0));

            var journeys = new Journey[] { journey1, journey2, journey3, journey4, journey5, journey6, journey7, journey8, journey9, journey10, journey11, journey12, journey13, journey14 };

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(420, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsExceedingDailyCap()
        {
            // Actor
            var week1MondayTrips = Enumerable.Range(1, 7).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1WednesDayTrips = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1ThursdayTrips = Enumerable.Range(1, 7).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1SundayTrips = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2MondayTrips = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2WednesDayTrips = Enumerable.Range(1, 3).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));

            var journeys = new List<Journey>();
            journeys.AddRange(week1MondayTrips);
            journeys.AddRange(week1WednesDayTrips);
            journeys.AddRange(week1ThursdayTrips);
            journeys.AddRange(week1SundayTrips);
            journeys.AddRange(week2MondayTrips);
            journeys.AddRange(week2WednesDayTrips);

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(600, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsNotExceedingWeeklyCap()
        {
            // Actor
            var week1MondayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1WednesDayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1ThursdayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1SundayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2MondayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2WednesDayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));

            var journeys = new List<Journey>();
            journeys.AddRange(week1MondayTrips);
            journeys.AddRange(week1WednesDayTrips);
            journeys.AddRange(week1ThursdayTrips);
            journeys.AddRange(week1SundayTrips);
            journeys.AddRange(week2MondayTrips);
            journeys.AddRange(week2WednesDayTrips);

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(720, totalFare);
        }

        [Fact]
        public void FareComputation_InterZoneTripsExceedingWeeklyCap()
        {
            // Actor
            var week1MondayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1WednesDayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1ThursdayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Thursday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1FridayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Friday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1SaturdayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Saturday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week1SundayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Sunday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2MondayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Monday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));
            var week2WednesDayTrips = Enumerable.Range(1, 9).Select(i => new Journey(Zones.Zone1, i % 2 == 0 ? Zones.Zone2 : Zones.Zone1, DayOfWeek.Wednesday, new TimeSpan(i % 2 == 0 ? 6 : 18, 0, 0)));

            var journeys = new List<Journey>();
            journeys.AddRange(week1MondayTrips);
            journeys.AddRange(week1WednesDayTrips);
            journeys.AddRange(week1ThursdayTrips);
            journeys.AddRange(week1FridayTrips);
            journeys.AddRange(week1SaturdayTrips);
            journeys.AddRange(week1SundayTrips);
            journeys.AddRange(week2MondayTrips);
            journeys.AddRange(week2WednesDayTrips);

            // Activity
            var totalFare = this.service.ComputeFares(journeys);

            // Assert
            Assert.Equal(840, totalFare);
        }
    }
}
