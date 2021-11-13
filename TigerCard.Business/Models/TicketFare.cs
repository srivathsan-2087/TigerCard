namespace TigerCard.Business.Models
{
    public class TicketFare 
    {
        public TicketFare(Zones origin, Zones destination, decimal peakFare, decimal nonPeakFare)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.PeakFare = peakFare;
            this.NonPeakFare = nonPeakFare;
        }

        public Zones Origin { get; }

        public Zones Destination { get; }

        public decimal PeakFare { get; }

        public decimal NonPeakFare { get; }
    }
}
