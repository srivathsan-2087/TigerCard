using System;
using TigerCard.Business.Interfaces;

namespace TigerCard.Business.Models
{
    public enum Zones
    {
        Zone1 = 1,
        Zone2 = 2
    }

    public class Journey
    {
        public Zones Origin { get; }

        public Zones Destination { get; }

        public DayOfWeek Day { get; }

        public TimeSpan Time { get; }

        public JourneyFare Fare { get; private set; }

        public Journey(Zones origin, Zones destination, DayOfWeek dayOfJourney, TimeSpan timeOfJourney)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.Day = dayOfJourney;
            this.Time = timeOfJourney;
        }

        public void CalculateFare(IFareCalculator calculator)
        {
            this.Fare = calculator.GetJourneyFare(this.Origin, this.Destination, this.Day, this.Time);
        }
    }
}
