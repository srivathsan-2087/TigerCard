using System;
using System.Collections.Generic;
using System.Linq;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business.Settings
{
    internal class FareSettings : IFareSettings
    {
        private readonly IDictionary<DayOfWeek, IEnumerable<Duration>> peakHours;
        private readonly IEnumerable<TicketFare> ticketFares;
        private readonly IEnumerable<FareCap> fareCaps;

        public FareSettings(IDictionary<DayOfWeek, IEnumerable<Duration>> peakHours, IEnumerable<TicketFare> ticketFares, IEnumerable<FareCap> fareCaps)
        {
            this.peakHours = peakHours;
            this.ticketFares = ticketFares;
            this.fareCaps = fareCaps;
        }

        public IEnumerable<Duration> GetPeakHoursForDayOfWeek(DayOfWeek dayOfWeek)
        {
            IEnumerable<Duration> peakHoursForDayOfWeek;

            if(!peakHours.TryGetValue(dayOfWeek, out peakHoursForDayOfWeek))
            {
                return Enumerable.Empty<Duration>();
            }

            return peakHoursForDayOfWeek.ToList();
        }

        public TicketFare GetTicketFare(Zones origin, Zones destination)
        {
            return this.ticketFares.First(r => ((r.Origin == origin && r.Destination == destination) || (r.Origin == destination && r.Destination == origin)));
        }

        public FareCap GetFareCap(Zones origin, Zones destination)
        {
            return this.fareCaps.First(r => ((r.Origin == origin && r.Destination == destination) || (r.Origin == destination && r.Destination == origin)));
        }
    }
}