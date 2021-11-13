using System;

using Moq;

using TigerCard.Business;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

using Xunit;

namespace TigerCard.Test
{
    public class FareCalculatorTests
    {
        private IFareCalculator calculator;
        private Mock<IFareSettings> fareSettingsMock;

        public FareCalculatorTests()
        {
            this.fareSettingsMock = new Mock<IFareSettings>();
            this.calculator = new FareCalculator(this.fareSettingsMock.Object);
        }

        [Fact]
        public void GetJourneyFare_WhenCalled_ShouldGetPeakHoursForGivenDayFromFareSettings()
        {
            // Actors
            var duration1 = new Duration(new TimeSpan(10, 0, 0), 120);
            var duration2 = new Duration(new TimeSpan(18, 0, 0), 150);

            this.fareSettingsMock.Setup(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday)).Returns(new[] { duration1, duration2 });
            this.fareSettingsMock.Setup(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2)).Returns(new TicketFare(Zones.Zone1, Zones.Zone2, 5m, 2.5m));

            // Activity
            var journeyFare = this.calculator.GetJourneyFare(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(11, 0, 0));

            // Asserts
            this.fareSettingsMock.Verify(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday), Times.Once);
        }

        [Fact]
        public void GetJourneyFare_WhenCalled_ShouldGetTicketFareFromFareSettings()
        {
            // Actors
            var duration1 = new Duration(new TimeSpan(10, 0, 0), 120);
            var duration2 = new Duration(new TimeSpan(18, 0, 0), 150);

            this.fareSettingsMock.Setup(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday)).Returns(new[] { duration1, duration2 });
            this.fareSettingsMock.Setup(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2)).Returns(new TicketFare(Zones.Zone1, Zones.Zone2, 5m, 2.5m));

            // Activity
            var journeyFare = this.calculator.GetJourneyFare(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(11, 0, 0));

            // Asserts
            this.fareSettingsMock.Verify(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2), Times.Once);
        }

        [Fact]
        public void GetJourneyFare_WhenJourneyIsDuringPeakHour_ShouldReturnJourneyFareWithPeakHourPrice()
        {
            // Actors
            var duration1 = new Duration(new TimeSpan(10, 0, 0), 120);
            var duration2 = new Duration(new TimeSpan(18, 0, 0), 150);

            this.fareSettingsMock.Setup(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday)).Returns(new[] { duration1, duration2 });
            this.fareSettingsMock.Setup(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2)).Returns(new TicketFare(Zones.Zone1, Zones.Zone2, 5m, 2.5m));

            // Activity
            var journeyFare = this.calculator.GetJourneyFare(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(11, 0, 0));

            // Asserts
            Assert.Equal(5m, journeyFare.Price);
        }

        [Fact]
        public void GetJourneyFare_WhenJourneyIsNotDuringPeakHour_ShouldReturnJourneyFareWithOffPeakHourPrice()
        {
            // Actors
            var duration1 = new Duration(new TimeSpan(10, 0, 0), 120);
            var duration2 = new Duration(new TimeSpan(18, 0, 0), 150);

            this.fareSettingsMock.Setup(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday)).Returns(new[] { duration1, duration2 });
            this.fareSettingsMock.Setup(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2)).Returns(new TicketFare(Zones.Zone1, Zones.Zone2, 5m, 2.5m));

            // Activity
            var journeyFare = this.calculator.GetJourneyFare(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(14, 0, 0));

            // Asserts
            Assert.Equal(2.5m, journeyFare.Price);
        }

        [Fact]
        public void GetJourneyFare_WhenPeakHourIsNotConfigured_ShouldReturnJourneyFareWithOffPeakHourPrice()
        {
            // Actors
            this.fareSettingsMock.Setup(m => m.GetPeakHoursForDayOfWeek(DayOfWeek.Monday)).Returns(new Duration[0]);
            this.fareSettingsMock.Setup(m => m.GetTicketFare(Zones.Zone1, Zones.Zone2)).Returns(new TicketFare(Zones.Zone1, Zones.Zone2, 5m, 2.5m));

            // Activity
            var journeyFare = this.calculator.GetJourneyFare(Zones.Zone1, Zones.Zone2, DayOfWeek.Monday, new TimeSpan(14, 0, 0));

            // Asserts
            Assert.Equal(2.5m, journeyFare.Price);
        }
    }
}
