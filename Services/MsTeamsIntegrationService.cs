using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ScheduleMsTeamsMeeting.Services
{
    public class MsTeamsIntegrationService()
    {
        /// <summary>
        /// An HttpClient instance configured for Microsoft Graph API requests.
        /// </summary>
        private readonly HttpClient _httpClient = GraphHttpClientService.Create();

        /// <summary>
        /// The access token used to authorize requests to Microsoft Graph API.
        /// </summary>
        private readonly string accessToken = "Test_accessToken";

        /// <summary>
        /// The email address of the current user making the request.
        /// </summary>
        private readonly string currentUserEmail = "currentUserEmail@example.com";

        /// <summary>
        /// Manages a calendar meeting by performing an action (Create, Update, or Delete) based on the provided request.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <returns></returns>
        public async Task<MeetingResponse> ManageCalendarMeeting(MeetingRequest requestMeeting)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(accessToken) || requestMeeting == null || string.IsNullOrEmpty(currentUserEmail))
                {
                    Console.WriteLine("Invalid input: Missing access token, meeting details, or current user email.");
                    return new MeetingResponse();
                }

                // Set the authorization header for the HTTP client
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                // Handle the event action (create, update, delete)
                HttpResponseMessage response = await HandleEventAction(requestMeeting, currentUserEmail);

                // If the response is successful, extract the event ID
                if (response.IsSuccessStatusCode)
                {
                    if (requestMeeting.MeetingAction == EnumMeetingAction.DeleteEvent)
                    {
                        return new MeetingResponse { MeetingId = requestMeeting.MeetingId ?? string.Empty };
                    }

                    return await ExtractMeetingDetailsResponse(response);
                }

                // Log an error if the operation fails
                Console.WriteLine(new Exception(string.Format("Failed to process meeting action: {0} - {1}", response.StatusCode, response.ReasonPhrase)));
                return new MeetingResponse();  // Returning empty response if the request failed
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"{ex} \n\nAn error occurred while managing the calendar meeting.");
                throw;  // Rethrow the exception after logging it
            }
        }

        /// <summary>
        /// Builds the meeting data from the meeting request details.
        /// </summary>
        private static StringContent BuildMeetingContent(MeetingRequest requestMeeting)
        {
            // Construct the meeting object with the request meeting details
            object meetingData = new
            {
                requestMeeting.Subject,
                Body = new
                {
                    ContentType = "HTML",
                    Content = requestMeeting.Description
                },
                Start = new
                {
                    DateTime = requestMeeting.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    requestMeeting.TimeZone
                },
                End = new
                {
                    DateTime = requestMeeting.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
                    requestMeeting.TimeZone
                },
                Location = new
                {
                    DisplayName = "Microsoft Teams"
                },
                Attendees = requestMeeting.MeetingParticipants.Select(i => new
                {
                    EmailAddress = new
                    {
                        Address = i.EmailAddress,
                        i.Name
                    },
                    Type = i.Type.ToString()
                }),
                requestMeeting.IsOnlineMeeting,
                OnlineMeetingProvider = "TeamsForBusiness"
            };

            // Serialize event data to JSON
            var json = JsonSerializer.Serialize(meetingData);

            return new StringContent(json, Encoding.UTF8, "application/json");
        }

        /// <summary>
        /// Handles the meeting action based on the action type (Create, Update, Delete).
        /// </summary>
        private async Task<HttpResponseMessage> HandleEventAction(MeetingRequest requestMeeting, string currentUserEmail)
        {
            return requestMeeting.MeetingAction switch
            {
                EnumMeetingAction.CreateNewEvent => await CreateMeeting(requestMeeting, currentUserEmail),
                EnumMeetingAction.UpdateEvent => await UpdateMeeting(requestMeeting, currentUserEmail),
                EnumMeetingAction.DeleteEvent => await DeleteMeeting(requestMeeting, currentUserEmail),
                _ => new HttpResponseMessage(HttpStatusCode.BadRequest)  // Return a bad request for unsupported actions
            };
        }

        /// <summary>
        /// Extracts the meeting ID from the response content if the request is successful.
        /// </summary>
        private static async Task<MeetingResponse> ExtractMeetingDetailsResponse(HttpResponseMessage response)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            string meetingId = string.Empty;

            // Parse the response JSON and extract the meeting ID
            using (var document = JsonDocument.Parse(responseContent))
            {
                var root = document.RootElement;
                if (root.TryGetProperty("id", out var meetingIdElement))
                {
                    meetingId = meetingIdElement.GetString() ?? string.Empty;
                }
            }

            // Return the meeting response with the meeting ID
            return new MeetingResponse { MeetingId = meetingId ?? string.Empty };
        }


        /// <summary>
        /// Creates a Microsoft Teams meeting using the provided meeting details and sends a POST request to the Microsoft Graph API to schedule the event.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> CreateMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Construct the meeting content with the create details
            var meetingcontent = BuildMeetingContent(requestMeeting);

            // Set the endpoint URL for Microsoft Graph API to create events
            string endpoint = $"/v1.0/users/{currentUserEmail}/events";

            // Send the HTTP POST request to create the meeting
            return await _httpClient.PostAsync(endpoint, meetingcontent);
        }


        /// <summary>
        /// Updates an existing Microsoft Teams meeting using the provided meeting details and sends a PATCH request to the Microsoft Graph API.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> UpdateMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Check if the MeetingId is null or empty in the request. 
            // If it is, return a BadRequest (400) response indicating that the request is invalid.
            if (string.IsNullOrEmpty(requestMeeting.MeetingId))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Construct the event object with the updated details
            var meetingcontent = BuildMeetingContent(requestMeeting);

            // Set the endpoint URL for Microsoft Graph API to create events
            string endpoint = $"/v1.0/users/{currentUserEmail}/events/{requestMeeting.MeetingId}";

            // Send the PATCH request to update the meeting
            return await _httpClient.PatchAsync(endpoint, meetingcontent);
        }


        /// <summary>
        /// Deletes an existing Microsoft Teams meeting by sending a DELETE request to the Microsoft Graph API.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> DeleteMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Check if the MeetingId is null or empty in the request. 
            // If it is, return a BadRequest (400) response indicating that the request is invalid.
            if (string.IsNullOrEmpty(requestMeeting.MeetingId))
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            // Set the endpoint URL for Microsoft Graph API to create events
            string endpoint = $"/v1.0/users/{currentUserEmail}/events/{requestMeeting.MeetingId}";

            // Delete the event by sending a DELETE request to the Microsoft Graph API
            return await _httpClient.DeleteAsync(endpoint);
        }

    }
}
