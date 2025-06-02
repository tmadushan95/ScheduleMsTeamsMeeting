namespace ScheduleMsTeamsMeeting.Services
{
    public static class GraphHttpClientService
    {
        /// <summary>
        /// Creates and configures an HttpClient instance with the base address set to Microsoft Graph.
        /// </summary>
        /// <returns>A configured HttpClient instance.</returns>
        public static HttpClient Create()
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://graph.microsoft.com")
            };

            // Optional: Add default headers, like Authorization or Accept
            // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "your-access-token");
            // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
