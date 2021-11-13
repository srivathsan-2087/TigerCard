namespace TigerCard.Business.Models
{
    public class JourneyFare
    {
        public decimal Price { get; private set; }

        public bool IsPeakHour { get; }

        public FareCapType AppliedCapType { get; private set; }

        public JourneyFare(bool isPeakHour, decimal price)
        {
            this.IsPeakHour = isPeakHour;
            this.Price = price;
            this.AppliedCapType = FareCapType.None;
        }

        public void ApplyCappedPrice(decimal price = 0, FareCapType capType = FareCapType.None)
        {
            this.Price = price;
            this.AppliedCapType = capType;
        }
    }
}