using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Helpers;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern
{
    public class DailyRecurrencePattern : IRecurrencePatternBuilder
    {
        /// <summary>
        /// Builds a recurrence pattern for Daily End Date.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        public object BuildRecurrencePattern(RecurrencePattern recurrencePattern)
        {
            // Validate RepeatingInterval
            if (recurrencePattern.RepeatingInterval < 1)
            {
                throw new ArgumentException("RepeatingInterval must be at least 1 for Daily recurrence pattern.");
            }

            // Build the pattern
            var pattern = new
            {
                Type = "daily",
                Interval = recurrencePattern.RepeatingInterval
            };

            // Build the range
            var range = RecurrencePatternHelper.BuildRecurrenceRange(recurrencePattern);

            return new
            {
                Pattern = pattern,
                Range = range
            };
        }
    }
}
