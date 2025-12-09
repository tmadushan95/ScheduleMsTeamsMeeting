using ScheduleMsTeamsMeeting.Models.Enums;

namespace ScheduleMsTeamsMeeting.Models
{
    /// <summary>
    /// Represents a request to schedule a meeting.
    /// </summary>
    public class MeetingRequest
    {
        /// <summary>
        /// Unique identifier for the meeting.
        /// </summary>
        public string MeetingId { get; set; } = null!;

        /// <summary>
        /// Unique identifier for a specific instance of a recurring meeting.
        /// </summary
        public string MeetingInstanceId { get; set; } = null!;

        /// <summary>
        /// The subject or title of the meeting.
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// An optional description of the meeting.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// The time zone in which the meeting is scheduled (e.g., "Pacific Standard Time").
        /// </summary>
        public string TimeZone { get; set; } = null!;

        /// <summary>
        /// Action to be taken for the meeting (e.g., create, update, cancel).
        /// </summary>
        public EnumMeetingAction MeetingAction { get; set; }

        /// <summary>
        /// The start date and time of the meeting.
        /// </summary>
        public DateTime StartDateTime { get; set; }

        /// <summary>
        /// The end date and time of the meeting.
        /// </summary>
        public DateTime EndDateTime { get; set; }

        /// <summary>
        /// Indicates whether the meeting is an online meeting 
        /// </summary>
        public bool IsOnlineMeeting { get; set; }

        /// <summary>
        /// A list of participants invited to the meeting.
        /// </summary>
        public List<MeetingParticipant> MeetingParticipants { get; set; } = null!;

        /// <summary>
        /// Indicates whether the meeting is reccurring.
        /// </summary>
        public bool IsReccurring { get; set; }

        /// <summary>
        /// The recurrence pattern for the meeting if it is reccurring.
        /// </summary>
        public RecurrencePattern? Reccurrence { get; set; }
    }

    /// <summary>
    /// Represents a participant in a meeting.
    /// </summary>
    public class MeetingParticipant
    {
        /// <summary>
        /// The name of the participant.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// The email address of the participant.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// The participant's role or type in the meeting (e.g., Required, Optional).
        /// </summary>
        public EnumMeetingParticipationType Type { get; set; }
    };

    public class RecurrencePattern
    {
        /// <summary>
        /// The start date of the recurrence pattern.
        /// </summary>
        public DateTime ReccurrenceStartDate { get; set; }

        /// <summary>
        /// The end date of the recurrence pattern.
        /// </summary>
        public DateTime ReccurrenceEndDate { get; set; }

        /// <summary>
        /// The type of recurrence pattern (e.g., Daily, Weekly, Monthly, Yearly).
        /// </summary>
        public EnumReccurrencePatternType ReccurrencePatternType { get; set; }

        /// <summary>
        /// The interval at which the event repeats.
        /// </summary>
        public int RepeatingInterval { get; set; }

        /// <summary>
        /// The number of occurrences for the recurrence pattern.
        /// </summary>
        public int NumberOfOccurrences { get; set; }

        /// <summary>
        /// The days of the week on which the event recurs (applicable for weekly and certain monthly patterns).
        /// </summary>
        public List<EnumWeekDay> ReccurrenceDaysOfWeek { get; set; } = null!;

        /// <summary>
        /// The type of recurrence range (e.g., NoEndDate, EndDate, NumberOfOccurrences).
        /// </summary>
        public EnumReccurrenceRangeType ReccurrenceRangeType { get; set; }

        /// <summary>
        /// The mode of editing the recurrence (e.g., ThisOccurrence, ThisAndFuture).
        /// </summary>
        public EnumRecureneceEditMode RecureneceEditMode { get; set; }

        /// <summary>
        /// The selected recurrence option for monthly and yearly patterns (e.g., OnFirst, OnSecond).
        /// </summary>
        public EnumSelectedRecurrenceOption SelectedRecurrenceOption { get; set; }
    }

}
