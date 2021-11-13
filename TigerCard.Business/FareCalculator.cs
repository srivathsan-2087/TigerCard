using System;
using System.Linq;

using TigerCard.Business.Extensions;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business
{
    internal class FareCalculator : IFareCalculator
    {
        IFareSettings fareSettings;

        public FareCalculator(IFareSettings fareSettings)
        {
            this.fareSettings = fareSettings;
        }

        public JourneyFare GetJourneyFare(Zones origin, Zones destination, DayOfWeek dayOfJourney, TimeSpan timeOfJourney)
        {
            var peakHours = this.fareSettings.GetPeakHoursForDayOfWeek(dayOfJourney).ToList();

            var isPeakHour = peakHours.Any() && peakHours.Any(duration => timeOfJourney.IsBetween(duration));

            var ticketFare = this.fareSettings.GetTicketFare(origin, destination);

            return new JourneyFare(isPeakHour, isPeakHour ? ticketFare.PeakFare : ticketFare.NonPeakFare);
        }
    }
}