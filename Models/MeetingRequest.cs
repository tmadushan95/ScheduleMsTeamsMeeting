using ScheduleMsTeamsMeeting.Models.Enums;

namespace ScheduleMsTeamsMeeting.Models
{
    /// <summary>
    /// Represents a request to schedule a meeting.
    /// </summary>
    public record MeetingRequest(
        /// <summary>
        /// Unique identifier for the meeting.
        /// </summary>
        string MeetingId,

        /// <summary>
        /// The subject or title of the meeting.
        /// </summary>
        string Subject,

        /// <summary>
        /// An optional description of the meeting.
        /// </summary>
        string? Description,

        /// <summary>
        /// The time zone in which the meeting is scheduled (e.g., "Pacific Standard Time").
        /// </summary>
        string TimeZone,

        /// <summary>
        /// Action to be taken for the meeting (e.g., create, update, cancel).
        /// </summary>
        EnumMeetingAction MeetingAction,

        /// <summary>
        /// The start date and time of the meeting.
        /// </summary>
        DateTime StartDateTime,

        /// <summary>
        /// The end date and time of the meeting.
        /// </summary>
        DateTime EndDateTime,

        /// <summary>
        /// Indicates whether the meeting is an online meeting 
        /// </summary>
        bool IsOnlineMeeting,

        /// <summary>
        /// A list of participants invited to the meeting.
        /// </summary>
        List<MeetingParticipant> MeetingParticipants
    );

    /// <summary>
    /// Represents a participant in a meeting.
    /// </summary>
    public record MeetingParticipant(
        /// <summary>
        /// The name of the participant.
        /// </summary>
        string Name,

        /// <summary>
        /// The email address of the participant.
        /// </summary>
        string EmailAddress,

        /// <summary>
        /// The participant's role or type in the meeting (e.g., Required, Optional).
        /// </summary>
        EnumMeetingParticipationType Type
    );

}
