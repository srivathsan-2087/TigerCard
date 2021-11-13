using System.Collections.Generic;

using TigerCard.Business.Models;

namespace TigerCard.Business.Grouping
{
    /// <summary>
    /// Group of the input journeys based on the specific strategy for splitting the input journeys. 
    /// </summary>
    public class JourneyGroup
    {
        public JourneyGroup(IEnumerable<Journey> inputJourneys)
        {
            this.Journeys = inputJourneys;
        }

        public IEnumerable<Journey> Journeys { get; }

        public void ApplyCap(decimal fareCap, FareCapType fareCapType)
        {
            decimal total = 0;

            foreach (var journey in this.Journeys)
            {
                if (total == fareCap)
                {
                    journey.Fare.ApplyCappedPrice(0, fareCapType);
                    continue;
                }

                if (total + journey.Fare.Price > fareCap)
                {
                    journey.Fare.ApplyCappedPrice(fareCap - total, fareCapType);
                    total = fareCap;
                    continue;
                }

                total += journey.Fare.Price;
            }
        }
    }
}
