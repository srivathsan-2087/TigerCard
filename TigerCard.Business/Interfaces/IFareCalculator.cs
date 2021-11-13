using System;

using TigerCard.Business.Models;

namespace TigerCard.Business.Interfaces
{
    public interface IFareCalculator
    {
        JourneyFare GetJourneyFare(Zones origin, Zones destination, DayOfWeek dayOfJourney, TimeSpan timeOfJourney);
    }
}