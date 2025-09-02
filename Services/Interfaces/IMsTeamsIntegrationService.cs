using ScheduleMsTeamsMeeting.Models;

namespace ScheduleMsTeamsMeeting.Services.Interfaces
{
    public interface IMsTeamsIntegrationService
    {
        /// <summary>
        /// Manages a calendar meeting by performing an action (Create, Update, or Delete)
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <returns></returns>
        Task<MeetingResponse> ManageCalendarMeeting(MeetingRequest requestMeeting);
    }
}
