namespace ScheduleMsTeamsMeeting.Models
{
    /// <summary>
    /// Represents the response returned after processing a meeting request.
    /// </summary>
    public class MeetingResponse
    {
        /// <summary>
        /// Gets or sets the unique identifier of the meeting.
        /// </summary>
        public string MeetingId { get; set; } = null!;
    }

}
