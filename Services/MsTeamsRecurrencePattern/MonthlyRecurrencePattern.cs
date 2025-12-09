using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Helpers;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern
{

    public class MonthlyRecurrencePattern : IRecurrencePatternBuilder
    {
        /// <summary>
        /// Builds a recurrence pattern for Monthly End Date.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        public object BuildRecurrencePattern(RecurrencePattern recurrencePattern)
        {
            // Determine if it's absolute or relative monthly
            bool isAbsoluteMonthly = recurrencePattern.SelectedRecurrenceOption == EnumSelectedRecurrenceOption.None;

            // Build the pattern based on absolute or relative monthly
            object pattern = isAbsoluteMonthly
                     ? BuildAbsoluteMonthlyPattern(recurrencePattern)
                     : BuildRelativeMonthlyPattern(recurrencePattern);

            // Build the range
            var range = RecurrencePatternHelper.BuildRecurrenceRange(recurrencePattern);

            return new
            {
                Pattern = pattern,
                Range = range
            };
        }

        /// <summary>
        /// Builds the absolute monthly pattern.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        private static object BuildAbsoluteMonthlyPattern(RecurrencePattern recurrencePattern)
        {
            int dayOfMonth = recurrencePattern.ReccurrenceStartDate.Day;

            return new
            {
                Type = "absoluteMonthly",
                Interval = recurrencePattern.RepeatingInterval,
                DayOfMonth = dayOfMonth
            };
        }

        /// <summary>
        /// Builds the relative monthly pattern.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <param name="daysOfWeek"></param>
        /// <returns></returns>
        private static object BuildRelativeMonthlyPattern(RecurrencePattern recurrencePattern)
        {
            // Convert EnumWeekDay to string array
            string[] daysOfWeek = recurrencePattern.ReccurrenceDaysOfWeek.Select(day => day.ToString()).ToArray();

            #region Validations
            // Validate that at least one day of the week is provided
            if (daysOfWeek.Length == 0)
            {
                throw new InvalidOperationException("Relative monthly recurrence pattern must have at least one day of the week.");
            }

            // Validate that only one day of the week is provided
            if (daysOfWeek.Length > 1)
            {
                throw new InvalidOperationException("Relative monthly recurrence pattern can only have one day of the week.");
            }

            // Validate repeating interval
            if (recurrencePattern.RepeatingInterval < 1)
            {
                throw new InvalidOperationException("Repeating interval must be at least 1 for a monthly recurrence pattern.");
            }

            #endregion

            // get the index (first, second, third, fourth, last)
            string index = RecurrencePatternHelper.GetWeekdayIndex(recurrencePattern.SelectedRecurrenceOption);

            return new
            {
                Type = "relativeMonthly",
                Interval = recurrencePattern.RepeatingInterval,
                DaysOfWeek = daysOfWeek,
                FirstDayOfWeek = "Sunday",
                Index = index
            };
        }
    }
}
