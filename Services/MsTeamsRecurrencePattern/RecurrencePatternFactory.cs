using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern
{
    public class RecurrencePatternFactory : IRecurrencePatternFactory
    {
        /// <summary>
        /// Builds a recurrence pattern based on the specified type.
        /// </summary>
        /// <param name="reccurrencePatternType"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public IRecurrencePatternBuilder GetRecurrenceBuilder(EnumReccurrencePatternType reccurrencePatternType)
        {
            return reccurrencePatternType switch
            {
                EnumReccurrencePatternType.Daily => new DailyRecurrencePattern(),
                EnumReccurrencePatternType.Weekly => new WeeklyRecurrencePattern(),
                EnumReccurrencePatternType.Monthly => new MonthlyRecurrencePattern(),
                EnumReccurrencePatternType.Yearly => new YearlyRecurrencePattern(),
                _ => throw new NotSupportedException($"Recurrence pattern type '{reccurrencePatternType}' is not supported.")
            };
        }
    }
}
