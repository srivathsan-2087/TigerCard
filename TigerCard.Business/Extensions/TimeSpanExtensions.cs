using System;

using TigerCard.Business.Models;

namespace TigerCard.Business.Extensions
{
    public static class TimeSpanExtensions
    {
        public static bool IsBetween(this TimeSpan timeSpan, Duration duration)
        {
            return duration.Start.TotalMinutes <= timeSpan.TotalMinutes && timeSpan.TotalMinutes <= duration.End.TotalMinutes;
        }
    }
}
