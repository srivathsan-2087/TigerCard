using System.Collections.Generic;

using TigerCard.Business.Grouping;
using TigerCard.Business.Models;

namespace TigerCard.Business.Interfaces
{
    /// <summary>
    /// Interface to group the journeys into different batches based on a criteria. 
    /// </summary>
    internal interface IJourneyGroupFareAggregator
    {
        IEnumerable<JourneyGroup> ComputeJourneyFare(IEnumerable<Journey> journeys);
    }
}
