using System;
using System.Collections.Generic;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business.Settings
{
    public static class FareSettingsFactory
    {
        public static IFareSettings CreateDefaultSettings()
        {
            return new FareSettings(InitializePeakHours(), InitializeTicketFare(), InitializeFareCap());
        }

        private static IDictionary<DayOfWeek, IEnumerable<Duration>> InitializePeakHours()
        {
            var dictionary = new Dictionary<DayOfWeek, IEnumerable<Duration>>();

            dictionary.Add(DayOfWeek.Monday, new[] { new Duration(new TimeSpan(7, 0, 0), new TimeSpan(10, 30, 0)), new Duration(new TimeSpan(17, 0, 0), new TimeSpan(20, 0, 0)) });
            dictionary.Add(DayOfWeek.Tuesday, new[] { new Duration(new TimeSpan(7, 0, 0), new TimeSpan(10, 30, 0)), new Duration(new TimeSpan(17, 0, 0), new TimeSpan(20, 0, 0)) });
            dictionary.Add(DayOfWeek.Wednesday, new[] { new Duration(new TimeSpan(7, 0, 0), new TimeSpan(10, 30, 0)), new Duration(new TimeSpan(17, 0, 0), new TimeSpan(20, 0, 0)) });
            dictionary.Add(DayOfWeek.Thursday, new[] { new Duration(new TimeSpan(7, 0, 0), new TimeSpan(10, 30, 0)), new Duration(new TimeSpan(17, 0, 0), new TimeSpan(20, 0, 0)) });
            dictionary.Add(DayOfWeek.Friday, new[] { new Duration(new TimeSpan(7, 0, 0), new TimeSpan(10, 30, 0)), new Duration(new TimeSpan(17, 0, 0), new TimeSpan(20, 0, 0)) });
            dictionary.Add(DayOfWeek.Saturday, new[] { new Duration(new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0)), new Duration(new TimeSpan(18, 0, 0), new TimeSpan(22, 0, 0)) });
            dictionary.Add(DayOfWeek.Sunday, new[] { new Duration(new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0)), new Duration(new TimeSpan(18, 0, 0), new TimeSpan(22, 0, 0)) });

            return dictionary;
        }

        private static IEnumerable<TicketFare> InitializeTicketFare()
        {
            List<TicketFare> ticketFares = new List<TicketFare>();

            ticketFares.Add(new TicketFare(Zones.Zone1, Zones.Zone1, 30, 25));
            ticketFares.Add(new TicketFare(Zones.Zone1, Zones.Zone2, 35, 30));
            ticketFares.Add(new TicketFare(Zones.Zone2, Zones.Zone2, 25, 20));

            return ticketFares;
        }

        private static IEnumerable<FareCap> InitializeFareCap()
        {
            Dictionary<FareCapType, decimal> inputLimits = new Dictionary<FareCapType, decimal>();
            inputLimits.Add(FareCapType.Daily, 100);
            inputLimits.Add(FareCapType.Weekly, 500);
            var capLimit1 = new FareCap(Zones.Zone1, Zones.Zone1, inputLimits);
            inputLimits.Clear();

            inputLimits.Add(FareCapType.Daily, 120);
            inputLimits.Add(FareCapType.Weekly, 600);
            var capLimit2 = new FareCap(Zones.Zone1, Zones.Zone2, inputLimits);
            inputLimits.Clear();

            inputLimits.Add(FareCapType.Daily, 80);
            inputLimits.Add(FareCapType.Weekly, 400);
            var capLimit3 = new FareCap(Zones.Zone2, Zones.Zone2, inputLimits);

            return new List<FareCap> { capLimit1, capLimit2, capLimit3 };
        }
    }
}
