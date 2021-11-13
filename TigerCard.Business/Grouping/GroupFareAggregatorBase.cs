using System;
using System.Collections.Generic;
using System.Linq;

using TigerCard.Business.Interfaces;
using TigerCard.Business.Models;

namespace TigerCard.Business.Grouping
{
    internal abstract class GroupFareAggregatorBase : IJourneyGroupFareAggregator
    {
        private readonly IFareSettings fareSettings;
        private readonly FareCapType fareCapType;

        protected GroupFareAggregatorBase(IFareSettings fareSettings, FareCapType fareCapType)
        {
            this.fareSettings = fareSettings;
            this.fareCapType = fareCapType;
        }

        public IEnumerable<JourneyGroup> ComputeJourneyFare(IEnumerable<Journey> journeys)
        {
            if (journeys == null || !journeys.Any())
            {
                return Enumerable.Empty<JourneyGroup>();
            }

            var journeyGroups = this.CreateGroups(journeys).ToList();

            foreach (var journeyGroup in journeyGroups)
            {                
                this.ComputeGroupFare(journeyGroup, this.fareCapType);

                var fareCap = this.GetCap(journeyGroup.Journeys);

                journeyGroup.ApplyCap(fareCap, fareCapType);
            }

            return journeyGroups;
        }
        /// <summary>
        /// Template Method to recursively group journeys, calculate capped fare
        /// </summary>
        /// <param name="journeyGroup">Journey group for which the fare must be computed</param>
        /// <param name="fareCapType">Fare Cap Type to be applied</param>
        protected abstract void ComputeGroupFare(JourneyGroup journeyGroup, FareCapType fareCapType);

        /// <summary>
        /// Template method to group journeys into groups.
        /// </summary>
        /// <param name="journeys">Journeys that must be grouped.</param>
        /// <returns>Enumerable of journeys grouped by certain scheme.</returns>
        protected abstract IEnumerable<JourneyGroup> CreateGroups(IEnumerable<Journey> journeys);

        /// <summary>
        /// Gets the fare cap limit that must applied for a group of journeys
        /// </summary>
        /// <param name="journeys">Journeys for which the cap must be determined.</param>
        /// <returns>The fare cap limit. Default behavior returns the cap based on the farthest journey.</returns>
        protected virtual decimal GetCap(IEnumerable<Journey> journeys)
        {
            var farthestJourney = journeys.GroupBy(j => Math.Abs(j.Destination - j.Origin))
                .OrderByDescending(grp => grp.Key)
                .First().First();

            var fareCap = this.fareSettings.GetFareCap(farthestJourney.Origin, farthestJourney.Destination);

            return fareCap.GetFareCap(this.fareCapType);
        }
    }
}
