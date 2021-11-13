using System;
using System.Collections.Generic;

namespace TigerCard.Business.Models
{
    public enum FareCapType
    {
        None = 0,
        Daily = 1,
        Weekly = 2
    }

    public class FareCap
    {
        private IDictionary<FareCapType, decimal> fareCaps;

        public FareCap(Zones origin, Zones destination, IDictionary<FareCapType, decimal> inputLimits)
        {
            if (inputLimits == null)
            {
                throw new ArgumentException("inputLimits cannot be null", "inputLimits");
            }

            this.Origin = origin;
            this.Destination = destination;

            fareCaps = new Dictionary<FareCapType, decimal>();

            foreach (FareCapType capType in inputLimits.Keys)
            {
                if(capType == FareCapType.None)
                {
                    continue;
                }

                fareCaps[capType] = inputLimits[capType];
            }
        }

        public Zones Origin { get; }

        public Zones Destination { get; }

        public decimal GetFareCap(FareCapType type)
        {
            return this.fareCaps[type];
        }
    }
}
