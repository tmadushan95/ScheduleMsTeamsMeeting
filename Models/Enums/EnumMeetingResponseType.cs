using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleMsTeamsMeeting.Models.Enums
{
    /// <summary>
    /// Defines the types of meeting response actions.
    /// </summary>
    public enum EnumMeetingResponseType
    {
        // Retrieval Operations
        MeetingNotFound = 0,
        MeetingRetrieved = 1,
        MeetingSeriesRetrieved = 2,

        // Individual Meeting Operations
        MeetingCreated = 3,
        MeetingUpdated = 4,
        MeetingDeleted = 5,

        // Recurring Meeting Operations
        RecurringMeetingCreated = 6,
        RecurringMeetingUpdated = 7,
        RecurringMeetingDeleted = 8
    }
}
