using System.Collections.Generic;

using TigerCard.Business.Models;

namespace TigerCard.Business.Interfaces
{
    public interface IFareComputationService
    {
        decimal ComputeFares(IEnumerable<Journey> journeys);
    }
}
