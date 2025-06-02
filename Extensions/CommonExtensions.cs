namespace ScheduleMsTeamsMeeting.Extensions
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
            DateTime targetDateTime = TimeZoneInfo.ConvertTime(date, sourceTimeZone, destinationTimeZone);

            return targetDateTime;
        }

    }
}
