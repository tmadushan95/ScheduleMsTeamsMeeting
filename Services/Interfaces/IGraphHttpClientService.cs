namespace ScheduleMsTeamsMeeting.Services.Interfaces
{
    public interface IGraphHttpClientService
    {
        /// <summary>
        /// Creates HttpClient instance with the base address set to Microsoft Graph.
        /// </summary>
        /// <returns></returns>
        HttpClient Create();
    }
}
