using System;

namespace TigerCard.Business.Models
{
    public class Duration
    {
        public TimeSpan Start { get; }

        public TimeSpan End { get; }

        public double Interval { get; }

        public Duration(TimeSpan start, double intervalInMinutes)
        {
            this.Start = start;
            this.Interval = Math.Abs(intervalInMinutes);
            this.End = TimeSpan.FromMinutes(this.Start.TotalMinutes + intervalInMinutes);
        }

        public Duration(TimeSpan start, TimeSpan end)
        {
            this.Start = start;
            this.End = end;

            this.Interval = this.End.Subtract(this.Start).TotalMinutes;
        }
    }
}
