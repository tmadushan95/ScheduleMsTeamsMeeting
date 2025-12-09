using ScheduleMsTeamsMeeting.Models.Enums;
using System.Text.Json.Serialization;

namespace ScheduleMsTeamsMeeting.Models
{
    /// <summary>
    /// Response for meeting creation
    /// </summary>
    public class MeetingResponse
    {
        public bool IsSuccess { get; set; }

        public EnumMeetingResponseType MeetingResponseType { get; set; }

        public string MeetingId { get; set; } = null!;

        public List<MeetingInstanceResponse>? MeetingInstances { get; set; }
    }


    /// <summary>
    /// Response for meeting instances
    /// </summary>
    public class InstancesResponse
    {
        [JsonPropertyName("value")]
        public List<MeetingInstanceResponse> Value { get; set; } = null!;

        [JsonPropertyName("@odata.nextLink")]
        public string? NextLink { get; set; }
    }

    /// <summary>
    /// Meeting instance details
    /// </summary>
    public class MeetingInstanceResponse
    {
        [JsonPropertyName("id")]
        public string InstanceId { get; set; } = null!;

        [JsonPropertyName("start")]
        public MeetingInstanceDateTimeResponse Start { get; set; } = null!;

        [JsonPropertyName("end")]
        public MeetingInstanceDateTimeResponse End { get; set; } = null!;

        [JsonPropertyName("recurrence")]
        public RecurrenceResponse? Recurrence { get; set; } = null!;

        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;
    }

    /// <summary>
    /// DateTime details for meeting instance
    /// </summary>
    public class MeetingInstanceDateTimeResponse
    {
        /// <summary>
        /// DateTime of the meeting instance
        /// </summary>
        [JsonPropertyName("dateTime")]
        public DateTime DateTime { get; set; }
    }

    /// <summary>
    /// Recurrence details
    /// </summary>
    public class RecurrenceResponse
    {
        [JsonPropertyName("pattern")]
        public RecurrencePatternResponse Pattern { get; set; } = null!;

        [JsonPropertyName("range")]
        public RecurrenceRangeResponse Range { get; set; } = null!;
    }

    /// <summary>
    /// Recurrence pattern details
    /// </summary>
    public class RecurrencePatternResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("interval")]
        public int Interval { get; set; }

        [JsonPropertyName("month")]
        public int Month { get; set; }

        [JsonPropertyName("dayOfMonth")]
        public int DayOfMonth { get; set; }

        [JsonPropertyName("daysOfWeek")]
        public List<string>? DaysOfWeek { get; set; }

        [JsonPropertyName("firstDayOfWeek")]
        public string FirstDayOfWeek { get; set; } = null!;

        [JsonPropertyName("index")]
        public string Index { get; set; } = null!;
    }

    /// <summary>
    /// Recurrence range details
    /// </summary>
    public class RecurrenceRangeResponse
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonPropertyName("startDate")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("recurrenceTimeZone")]
        public string RecurrenceTimeZone { get; set; } = null!;

        [JsonPropertyName("numberOfOccurrences")]
        public int NumberOfOccurrences { get; set; }
    }
}
