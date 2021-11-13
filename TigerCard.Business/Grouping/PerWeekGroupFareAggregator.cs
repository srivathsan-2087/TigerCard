using System;
using System.Collections.Generic;
using System.Linq;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business.Grouping
{
    internal class PerWeekGroupFareAggregator : GroupFareAggregatorBase
    {
        private readonly IJourneyGroupFareAggregator perDayAggregator;

        public PerWeekGroupFareAggregator(IFareSettings fareSettings, IFareCalculator fareCalculator) : base(fareSettings, FareCapType.Weekly)
        {
            this.perDayAggregator = new PerDayGroupFareAggregator(fareSettings, fareCalculator);
        }

        protected override void ComputeGroupFare(JourneyGroup journeyGroup, FareCapType fareCapType)
        {
            this.perDayAggregator.ComputeJourneyFare(journeyGroup.Journeys);
        }

        protected override IEnumerable<JourneyGroup> CreateGroups(IEnumerable<Journey> journeys)
        {
            if (journeys == null || !journeys.Any())
            {
                return Enumerable.Empty<JourneyGroup>();
            }

            var tripsArray = journeys.ToArray();

            var journeyGroup = new List<Journey>();
            var journeyGroups = new List<JourneyGroup>();
            var i = 0;

            for (; i < tripsArray.Length;)
            {
                if (tripsArray[i].Day != DayOfWeek.Sunday)
                {
                    journeyGroup.Add(tripsArray[i++]);
                    continue;
                }

                while (i < tripsArray.Length && tripsArray[i].Day == DayOfWeek.Sunday)
                {
                    journeyGroup.Add(tripsArray[i++]);
                }

                journeyGroups.Add(new JourneyGroup(journeyGroup.ToArray()));
                journeyGroup.Clear();
            }

            if (tripsArray[i - 1].Day != DayOfWeek.Sunday)
            {
                journeyGroups.Add(new JourneyGroup(journeyGroup.ToArray()));
                journeyGroup.Clear();
            }

            return journeyGroups;
        }
    }
}
