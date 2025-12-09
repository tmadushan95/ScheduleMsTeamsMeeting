using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Helpers
{
    /// <summary>
    /// Helper class for Recurrence Pattern related operations.
    /// </summary>
    public static class RecurrencePatternHelper
    {
        /// <summary>
        /// Gets the weekday index string based on the selected recurrence option.
        /// </summary>
        /// <param name="recurrenceOption"></param>
        /// <returns></returns>
        public static string GetWeekdayIndex(EnumSelectedRecurrenceOption recurrenceOption)
        {
            return recurrenceOption switch
            {
                EnumSelectedRecurrenceOption.OnFirst => "first",
                EnumSelectedRecurrenceOption.OnSecond => "second",
                EnumSelectedRecurrenceOption.OnThird => "third",
                EnumSelectedRecurrenceOption.OnFourth => "fourth",
                EnumSelectedRecurrenceOption.OnLast => "last",
                _ => string.Empty,
            };
        }

        /// <summary>
        /// Builds the recurrence range object based on the recurrence pattern.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        public static object BuildRecurrenceRange(RecurrencePattern recurrencePattern)
        {
            // Add null check for recurrencePattern
            if (recurrencePattern == null)
            {
                throw new ArgumentNullException(nameof(recurrencePattern));
            }

            // Validate ReccurrenceRangeType
            if (!Enum.IsDefined(typeof(EnumReccurrenceRangeType), recurrencePattern.ReccurrenceRangeType))
            {
                throw new InvalidOperationException("RecurrenceRangeType must be a valid value in the RecurrencePattern.");
            }

            // Validate ReccurrenceStartDate
            if (recurrencePattern.ReccurrenceStartDate == default)
            {
                throw new InvalidOperationException("ReccurrenceStartDate must be specified in the RecurrencePattern.");
            }

            // Validate ReccurrenceEndDate if ReccurrenceRangeType is EndDate
            if (recurrencePattern.ReccurrenceRangeType == EnumReccurrenceRangeType.EndDate && recurrencePattern.ReccurrenceStartDate > recurrencePattern.ReccurrenceEndDate)
            {
                // Validate ReccurrenceEndDate
                if (recurrencePattern.ReccurrenceEndDate == default)
                {
                    throw new InvalidOperationException("ReccurrenceEndDate must be specified in the RecurrencePattern when ReccurrenceRangeType is EndDate.");
                }

                // Validate that EndDate is after StartDate
                if (recurrencePattern.ReccurrenceStartDate > recurrencePattern.ReccurrenceEndDate)
                {
                    throw new InvalidOperationException("ReccurrenceEndDate must be after ReccurrenceStartDate in the RecurrencePattern.");
                }
            }

            // Validate NumberOfOccurrences if ReccurrenceRangeType is NumberOfOccurrence
            if (recurrencePattern.ReccurrenceRangeType == EnumReccurrenceRangeType.NumberOfOccurrence && recurrencePattern.NumberOfOccurrences <= 0)
            {
                throw new ArgumentException("NumberOfOccurrences must be greater than zero for a numbered recurrence range.", nameof(recurrencePattern));
            }

            // Build the range based on ReccurrenceRangeType
            return recurrencePattern.ReccurrenceRangeType switch
            {
                EnumReccurrenceRangeType.EndDate => new
                {
                    Type = "endDate",
                    StartDate = recurrencePattern.ReccurrenceStartDate.ToString("yyyy-MM-dd"),
                    EndDate = recurrencePattern.ReccurrenceEndDate.ToString("yyyy-MM-dd"),
                },
                EnumReccurrenceRangeType.NumberOfOccurrence => new
                {
                    Type = "numbered",
                    StartDate = recurrencePattern.ReccurrenceStartDate.ToString("yyyy-MM-dd"),
                    recurrencePattern.NumberOfOccurrences
                },
                _ => new
                {
                    Type = "noEnd",
                    StartDate = recurrencePattern.ReccurrenceStartDate.ToString("yyyy-MM-dd"),
                }
            };
        }
    }
}
