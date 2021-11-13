using System;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;
using TigerCard.Business.Settings;

using Xunit;

namespace TigerCard.Test.Settings
{
    public class FareSettingsTests
    {
        private IFareSettings fareSettings;

        public FareSettingsTests()
        {
            this.fareSettings = FareSettingsFactory.CreateDefaultSettings();
        }

        [Theory]
        [InlineData(DayOfWeek.Monday)]
        [InlineData(DayOfWeek.Tuesday)]
        [InlineData(DayOfWeek.Wednesday)]
        [InlineData(DayOfWeek.Thursday)]
        [InlineData(DayOfWeek.Friday)]
        [InlineData(DayOfWeek.Saturday)]
        [InlineData(DayOfWeek.Sunday)]
        public void GetPeakHoursForDayOfWeek_WhenCalled_ShouldNotReturnNullForAnyDayOfWeek(DayOfWeek dayOfWeek)
        {
            // Activity
            var peakHours = this.fareSettings.GetPeakHoursForDayOfWeek(dayOfWeek);

            // Asserts
            Assert.NotNull(peakHours);
        }

        [Theory]
        [InlineData(Zones.Zone1, Zones.Zone1)]
        [InlineData(Zones.Zone1, Zones.Zone2)]
        [InlineData(Zones.Zone2, Zones.Zone1)]
        [InlineData(Zones.Zone2, Zones.Zone2)]
        public void GetTicketFare_WhenCalled_ShouldReturnFareForAJourneyBetweenTwoZones(Zones origin, Zones destination)
        {
            // Activity
            var ticketFare = this.fareSettings.GetTicketFare(origin, destination);

            // Asserts
            Assert.NotNull(ticketFare);
        }

        [Theory]
        [InlineData(Zones.Zone1, Zones.Zone1)]
        [InlineData(Zones.Zone1, Zones.Zone2)]
        [InlineData(Zones.Zone2, Zones.Zone1)]
        [InlineData(Zones.Zone2, Zones.Zone2)]
        public void GetFareCap_WhenCalled_ShouldReturnFareCapForAJourneyBetweenTwoZones(Zones origin, Zones destination)
        {
            // Activity
            var fareCap = this.fareSettings.GetFareCap(origin, destination);

            // Asserts
            Assert.NotNull(fareCap);
        }
    }
}
