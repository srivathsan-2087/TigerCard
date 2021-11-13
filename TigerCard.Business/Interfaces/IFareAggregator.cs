using System.Collections.Generic;

using TigerCard.Business.Models;

namespace TigerCard.Business.Interfaces
{
    public interface IFareAggregator
    {
        decimal AggregateTotalFare(IEnumerable<Journey> journeys);
    }
}