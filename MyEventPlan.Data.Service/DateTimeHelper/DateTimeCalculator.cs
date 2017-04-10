using System;

namespace MyEventPlan.Data.Service.DateTimeHelper
{
    public class DateTimeCalculator
    {
        /// <summary>
        /// This mehod calculates the time a news was posted form the present time
        /// </summary>
        /// <param name="dateTimeValue"></param>
        /// <returns></returns>
        public string TimeAgo(DateTime dateTimeValue)
        {
            var span = DateTime.Now - dateTimeValue;
            if (span.Days > 365)
            {
                var years = span.Days/365;
                if (span.Days%365 != 0)
                    years += 1;
                return $"{years} {(years == 1 ? "year" : "years")} until the event";
            }
            if (span.Days > 30)
            {
                var months = span.Days/30;
                if (span.Days%31 != 0)
                    months += 1;
                return $"about {months} {(months == 1 ? "month" : "months")} until the event";
            }
            if (span.Days > 0)
                return $"{span.Days} {(span.Days == 1 ? "day" : "days")} until the event";
            if (span.Hours > 0)
                return $"{span.Hours} {(span.Hours == 1 ? "hour" : "hours")} until the event";
            if (span.Minutes > 0)
                return $"{span.Minutes} {(span.Minutes == 1 ? "minute" : "minutes")} until the event";
            if (span.Seconds > 5)
                return $"{span.Seconds} seconds ago";
            if (span.Seconds <= 5)
                return "until the event";
            return string.Empty;
        }
    }
}