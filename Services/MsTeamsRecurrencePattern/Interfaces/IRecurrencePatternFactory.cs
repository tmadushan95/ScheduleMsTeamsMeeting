using ScheduleMsTeamsMeeting.Models.Enums;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces
{
    public interface IRecurrencePatternFactory
    {
        /// <summary>
        /// Builds a recurrence pattern based on the specified type.
        /// </summary>
        /// <param name="reccurrencePatternType"></param>
        /// <returns></returns>
        IRecurrencePatternBuilder GetRecurrenceBuilder(EnumReccurrencePatternType reccurrencePatternType);
    }
}
