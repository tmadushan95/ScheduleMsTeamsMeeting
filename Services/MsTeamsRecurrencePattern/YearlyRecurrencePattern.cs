using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Helpers;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern
{
    public class YearlyRecurrencePattern : IRecurrencePatternBuilder
    {
        /// <summary>
        /// Builds a recurrence pattern for Yearly.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        public object BuildRecurrencePattern(RecurrencePattern recurrencePattern)
        {
            #region Validations
            // Validate repeating interval
            if (recurrencePattern.RepeatingInterval < 1)
            {
                throw new InvalidOperationException("Repeating interval must be at least 1 for a yearly recurrence pattern.");
            }
            #endregion

            // Determine if it's absolute or relative yearly
            bool isAbsoluteYearly = recurrencePattern.SelectedRecurrenceOption == EnumSelectedRecurrenceOption.None;

            // Build the pattern based on absolute or relative yearly
            object pattern = isAbsoluteYearly
                ? BuildAbsoluteYearlyPattern(recurrencePattern)
                : BuildRelativeYearlyPattern(recurrencePattern);

            // Build the range
            var range = RecurrencePatternHelper.BuildRecurrenceRange(recurrencePattern);

            return new
            {
                Pattern = pattern,
                Range = range
            };
        }

        /// <summary>
        /// Builds the absolute yearly recurrence pattern (e.g., every March 15th).
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        private static object BuildAbsoluteYearlyPattern(RecurrencePattern recurrencePattern)
        {
            // Get month and day from ReccurrenceStartDate
            int monthOfYear = recurrencePattern.ReccurrenceStartDate.Month;
            int dayOfMonth = recurrencePattern.ReccurrenceStartDate.Day;


            return new
            {
                Type = "absoluteYearly",
                Interval = recurrencePattern.RepeatingInterval,
                Month = monthOfYear,
                DayOfMonth = dayOfMonth
            };
        }

        /// <summary>
        /// Builds the relative yearly recurrence pattern (e.g., the second Monday of March every year).
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        private static object BuildRelativeYearlyPattern(RecurrencePattern recurrencePattern)
        {
            // Convert EnumWeekDay to string array
            string[] daysOfWeek = recurrencePattern.ReccurrenceDaysOfWeek.Select(day => day.ToString()).ToArray();

            #region Validations
            // Validate that at least one day of the week is provided
            if (daysOfWeek.Length == 0)
            {
                throw new ArgumentException("At least one day of the week must be specified for relative yearly recurrence patterns.");
            }

            // Validate that only one day of the week is provided
            if (daysOfWeek.Length > 1)
            {
                throw new InvalidOperationException("Relative yearly recurrence pattern can only have one day of the week.");
            }

            #endregion

            // get the index (first, second, third, fourth, last)
            string index = RecurrencePatternHelper.GetWeekdayIndex(recurrencePattern.SelectedRecurrenceOption);

            // Get month from ReccurrenceStartDate
            int monthOfYear = recurrencePattern.ReccurrenceStartDate.Month;

            return new
            {
                Type = "relativeYearly",
                Interval = recurrencePattern.RepeatingInterval,
                Month = monthOfYear,
                DaysOfWeek = daysOfWeek,
                Index = index
            };
        }

    }
}
