using System.Text;
using System.Text.Json;

namespace ScheduleMsTeamsMeeting.Services.Extensions
{
    public static class CommonExtensions
    {
        /// <summary>
        /// Converts a given <see cref="DateTime"/> from one time zone to another.
        /// </summary>
        /// <param name="date">The <see cref="DateTime"/> to convert.</param>
        /// <param name="sourceTimeZone">The source <see cref="TimeZoneInfo"/> representing the current time zone of the <paramref name="date"/>.</param>
        /// <param name="destinationTimeZone">The target <see cref="TimeZoneInfo"/> to convert the <paramref name="date"/> to.</param>
        /// <returns>A <see cref="DateTime"/> converted to the <paramref name="destinationTimeZone"/>.</returns>
        public static DateTime ConvertDateToTimeZone(this DateTime date, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
        {
            // Convert the DateTime from Local to the target time zone
            // If unspecified, assume it is in source timezone
            if (date.Kind == DateTimeKind.Unspecified)
            {
                date = DateTime.SpecifyKind(date, DateTimeKind.Unspecified);
                date = TimeZoneInfo.ConvertTime(date, sourceTimeZone, destinationTimeZone);
                return date;
            }
            DateTime targetDateTime = TimeZoneInfo.ConvertTime(date, sourceTimeZone, destinationTimeZone);

            return targetDateTime;
        }

        /// <summary>
        /// Converts an object to JSON StringContent for HTTP requests.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        public static StringContent ToJsonContent<T>(this T data)
        {
            string json = JsonSerializer.Serialize(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }

    }
}
