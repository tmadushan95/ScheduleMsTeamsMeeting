using ScheduleMsTeamsMeeting.Models;

namespace ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces
{
    public interface IRecurrencePatternBuilder
    {
        /// <summary>
        /// Builds a recurrence pattern object based on the provided RecurrencePattern.
        /// </summary>
        /// <param name="recurrencePattern"></param>
        /// <returns></returns>
        object BuildRecurrencePattern(RecurrencePattern recurrencePattern);
    }
}
