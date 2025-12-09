
using Microsoft.Extensions.DependencyInjection;
using ScheduleMsTeamsMeeting.Extensions;
using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services;
using ScheduleMsTeamsMeeting.Services.Interfaces;

#region Service Provider Configuration
/// <summary>
/// Registers application services and builds the service provider.
/// </summary>
var serviceProvider = new ServiceCollection()
                .AddSingleton<IGraphHttpClientService, GraphHttpClientService>()
                .AddScoped<IMsTeamsIntegrationService, MsTeamsIntegrationService>()
                .BuildServiceProvider();
#endregion

#region Resolves required application services from the configured
IMsTeamsIntegrationService _teamsIntegrationService = serviceProvider.GetRequiredService<IMsTeamsIntegrationService>();
#endregion

#region Create and Schedule Teams Meeting

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
    new()
    {
        Name= "Test User 1",
        EmailAddress="testUser1@example.com",
        Type=EnumMeetingParticipationType.Required
    },
    new()
    {
        Name= "Test User 2",
        EmailAddress="testUser2@example.com",
        Type=EnumMeetingParticipationType.Optional
    },
    new()
    {
        Name= "Test User 3",
        EmailAddress="testUser3@example.com",
        Type=EnumMeetingParticipationType.Required
    },
};

// Define recurrence pattern if the meeting is reccurring
bool isReccurring = false;

// Define recurrence details if the meeting is reccurring
RecurrencePattern? reccurrence = isReccurring ? new()
{
    ReccurrenceStartDate = startDateTime,
    ReccurrenceEndDate = endDateTime,
    ReccurrencePatternType = EnumReccurrencePatternType.Daily,
    RepeatingInterval = 1,
    NumberOfOccurrences = 0,
    ReccurrenceDaysOfWeek = new(),
    ReccurrenceRangeType = EnumReccurrenceRangeType.EndDate,
    RecureneceEditMode = EnumRecureneceEditMode.ThisOccurrence,
    SelectedRecurrenceOption = EnumSelectedRecurrenceOption.None
} : null;

// NOTE: For updating or deleting a meeting, the MeetingId must be provided.
// Use an empty string for creating a new meeting.
var meetingData = new MeetingRequest
{
    MeetingId = string.Empty,
    Subject = "Test Ms Teams Meeting",
    Description = "Test Ms Teams Meeting description",
    TimeZone = timezone,
    MeetingAction = EnumMeetingAction.CreateNewEvent,
    StartDateTime = startDateTime,
    EndDateTime = endDateTime,
    IsOnlineMeeting = true,
    MeetingParticipants = meetingParticipants,
    IsReccurring = isReccurring,
    Reccurrence = reccurrence
};

// Create the meeting using the integration service
MeetingResponse response = await _teamsIntegrationService.ManageCalendarMeeting(meetingData);
Console.WriteLine(response.MeetingId);

// You can now use response.MeetingId, etc., to confirm or store the meeting details.
#endregion
