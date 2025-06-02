
using ScheduleMsTeamsMeeting.Extensions;
using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services;



// Define the target time zone for the meeting (e.g., "Asia/Colombo")
string timezone = "Asia/Colombo";

// Define source and target time zone information
// Source is UTC; target is the selected time zone
TimeZoneInfo sourceTimeZone = TimeZoneInfo.Utc;
TimeZoneInfo targetTimeZone = TimeZoneInfo.FindSystemTimeZoneById(timezone);

// Convert current UTC start and end times to the selected time zone
DateTime startDateTime = DateTime.UtcNow.ConvertDateToTimeZone(sourceTimeZone, targetTimeZone);
DateTime endDateTime = DateTime.UtcNow.AddHours(2).ConvertDateToTimeZone(sourceTimeZone, targetTimeZone);

// Define participants for the meeting
var meetingParticipants = new List<MeetingParticipant>
{
    new("Test User 1", "testUser1@example.com", EnumMeetingParticipationType.Required),
    new("Test User 2", "testUser2@example.com", EnumMeetingParticipationType.Optional),
    new("Test User 3", "testUser3@example.com", EnumMeetingParticipationType.Required)
};

// NOTE: For updating or deleting a meeting, the MeetingId must be provided.
// Use an empty string for creating a new meeting.
var meetingData = new MeetingRequest
(
    string.Empty,                            // MeetingId (empty for new meeting)
    "Test Ms Teams Meeting",                 // Subject
    "Test Ms Teams Meeting description",     // Description
    timezone,                                // TimeZone
    EnumMeetingAction.CreateNewEvent,        // MeetingAction
    startDateTime,                           // StartDateTime
    endDateTime,                             // EndDateTime
    true,                                    // IsOnlineMeeting
    meetingParticipants                      // Participants
);

// Create the meeting using the integration service
MeetingResponse response = await new MsTeamsIntegrationService().ManageCalendarMeeting(meetingData);
Console.WriteLine(response.MeetingId);

// You can now use response.MeetingId, etc., to confirm or store the meeting details.

