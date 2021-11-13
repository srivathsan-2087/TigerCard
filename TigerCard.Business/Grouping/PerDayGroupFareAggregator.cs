using System.Collections.Generic;
using System.Linq;

using TigerCard.Business.Models;

namespace TigerCard.Business.Grouping
{
    using TigerCard.Business.Interfaces;

    internal class PerDayGroupFareAggregator : GroupFareAggregatorBase
    {
        private IFareCalculator fareCalculator;

        public PerDayGroupFareAggregator(IFareSettings settings, IFareCalculator fareCalculator) : base(settings, FareCapType.Daily)
        {
            this.fareCalculator = fareCalculator;
        }

        protected override void ComputeGroupFare(JourneyGroup journeyGroup, FareCapType fareCapType)
        {
            foreach (var journey in journeyGroup.Journeys)
            {
                journey.CalculateFare(this.fareCalculator);
            }
        }

        protected override IEnumerable<JourneyGroup> CreateGroups(IEnumerable<Journey> journeys)
        {
            return journeys.GroupBy(j => j.Day)
                           .Select(grp => new JourneyGroup(grp.OrderBy(j => j.Time)))
                           .ToList();
        }
    }
}
