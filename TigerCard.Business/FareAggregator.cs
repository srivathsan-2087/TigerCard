using System.Collections.Generic;
using System.Linq;

using TigerCard.Business.Grouping;
using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business
{
    internal class FareAggregator : IFareAggregator
    {
        private readonly IFareSettings fareSettings;
        private readonly IFareCalculator fareCalculator;

        public FareAggregator(IFareSettings fareSettings, IFareCalculator fareCalculator)
        {
            this.fareSettings = fareSettings;
            this.fareCalculator = fareCalculator;
        }

        public decimal AggregateTotalFare(IEnumerable<Journey> journeys)
        {
            if (journeys == null || !journeys.Any())
            {
                return 0m;
            }

            var perWeekGroupFareAggregator = new PerWeekGroupFareAggregator(this.fareSettings, this.fareCalculator);

            var journeyGroups = perWeekGroupFareAggregator.ComputeJourneyFare(journeys);

            var journeyGroupTotals = journeyGroups.Select(grp => grp.Journeys.Sum(j => j.Fare.Price));

            return journeyGroupTotals.Sum();
        }
    }
}