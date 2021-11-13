using System;
using System.Collections.Generic;

using TigerCard.Business.Models;

namespace TigerCard.Business.Interfaces
{
    public interface IFareSettings
    {
        IEnumerable<Duration> GetPeakHoursForDayOfWeek(DayOfWeek dayOfWeek);

        TicketFare GetTicketFare(Zones origin, Zones destination);

        FareCap GetFareCap(Zones origin, Zones destination);
    }
}
