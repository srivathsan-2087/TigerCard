using System.Collections.Generic;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business
{
    internal class FareComputationService : IFareComputationService
    {
        private readonly IFareAggregator fareAggregator;

        public FareComputationService(IFareAggregator fareAggregator)
        {
            this.fareAggregator = fareAggregator;
        }

        public decimal ComputeFares(IEnumerable<Journey> journeys)
        {
            return this.fareAggregator.AggregateTotalFare(journeys);
        }
    }
}