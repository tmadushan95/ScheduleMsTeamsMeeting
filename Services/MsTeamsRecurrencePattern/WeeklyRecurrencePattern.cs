using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Helpers;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern
{
    public class WeeklyRecurrencePattern : IRecurrencePatternBuilder
    {
        /// <summary>
        /// Builds a recurrence patternfor Weekly End Date.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        public object BuildRecurrencePattern(RecurrencePattern recurrencePattern)
        {
            // Convert EnumWeekDay to string array
            string[] daysOfWeek = recurrencePattern.ReccurrenceDaysOfWeek.Select(day => day.ToString()).ToArray();

            // Ensure at least one day of the week is specified
            if (daysOfWeek.Length == 0)
            {
                throw new InvalidOperationException("At least one day of the week must be specified for a weekly recurrence pattern.");
            }

            // Validate repeating interval
            if (recurrencePattern.RepeatingInterval < 1)
            {
                throw new InvalidOperationException("Repeating interval must be at least 1 for a weekly recurrence pattern.");
            }

            // Build the pattern
            var pattern = new
            {
                Type = "weekly",
                Interval = recurrencePattern.RepeatingInterval,
                DaysOfWeek = daysOfWeek
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
