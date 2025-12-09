using ScheduleMsTeamsMeeting.Models;
using ScheduleMsTeamsMeeting.Models.Enums;
using ScheduleMsTeamsMeeting.Services.Extensions;
using ScheduleMsTeamsMeeting.Services.Interfaces;
using ScheduleMsTeamsMeeting.Services.MsTeamsRecurrencePattern.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ScheduleMsTeamsMeeting.Services
{
    public class MsTeamsIntegrationService(IGraphHttpClientService httpClientService,IRecurrencePatternFactory recurrencePatternFactory): IMsTeamsIntegrationService
    {
        /// <summary>
        /// An HttpClient instance configured for Microsoft Graph API requests.
        /// </summary>
        private readonly HttpClient _httpClient = httpClientService.Create();

        /// <summary>
        /// The access token used to authorize requests to Microsoft Graph API.
        /// </summary>
        private readonly string accessToken = "Test_accessToken";

        /// <summary>
        /// The email address of the current user making the request.
        /// </summary>
        private readonly string currentUserEmail = "currentUserEmail@example.com";

        private readonly IRecurrencePatternFactory _recurrencePatternFactory = recurrencePatternFactory;

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
                return await HandleEventAction(requestMeeting, currentUserEmail);
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine($"An error occurred while managing the calendar meeting.{ex}");
                throw;  // Rethrow the exception after logging it
            }
        }

        /// <summary>
        /// Builds the meeting data from the meeting request details.
        /// </summary>
        //private static StringContent BuildMeetingContent(MeetingRequest requestMeeting)
        //{
        //    // Construct the meeting object with the request meeting details
        //    object meetingData = new
        //    {
        //        requestMeeting.Subject,
        //        Body = new
        //        {
        //            ContentType = "HTML",
        //            Content = requestMeeting.Description
        //        },
        //        Start = new
        //        {
        //            DateTime = requestMeeting.StartDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
        //            requestMeeting.TimeZone
        //        },
        //        End = new
        //        {
        //            DateTime = requestMeeting.EndDateTime.ToString("yyyy-MM-ddTHH:mm:ss"),
        //            requestMeeting.TimeZone
        //        },
        //        Location = new
        //        {
        //            DisplayName = "Microsoft Teams"
        //        },
        //        Attendees = requestMeeting.MeetingParticipants.Select(i => new
        //        {
        //            EmailAddress = new
        //            {
        //                Address = i.EmailAddress,
        //                i.Name
        //            },
        //            Type = i.Type.ToString()
        //        }),
        //        requestMeeting.IsOnlineMeeting,
        //        OnlineMeetingProvider = "TeamsForBusiness"
        //    };

        //    // Serialize event data to JSON
        //    var json = JsonSerializer.Serialize(meetingData);

        //    return new StringContent(json, Encoding.UTF8, "application/json");
        //}

        ///// <summary>
        ///// Handles the meeting action based on the action type (Create, Update, Delete).
        ///// </summary>
        //private async Task<HttpResponseMessage> HandleEventAction(MeetingRequest requestMeeting, string currentUserEmail)
        //{
        //    return requestMeeting.MeetingAction switch
        //    {
        //        EnumMeetingAction.CreateNewEvent => await CreateMeeting(requestMeeting, currentUserEmail),
        //        EnumMeetingAction.UpdateEvent => await UpdateMeeting(requestMeeting, currentUserEmail),
        //        EnumMeetingAction.DeleteEvent => await DeleteMeeting(requestMeeting, currentUserEmail),
        //        _ => new HttpResponseMessage(HttpStatusCode.BadRequest)  // Return a bad request for unsupported actions
        //    };
        //}

        ///// <summary>
        ///// Extracts the meeting ID from the response content if the request is successful.
        ///// </summary>
        //private static async Task<MeetingResponse> ExtractMeetingDetailsResponse(HttpResponseMessage response)
        //{
        //    var responseContent = await response.Content.ReadAsStringAsync();
        //    string meetingId = string.Empty;

        //    // Parse the response JSON and extract the meeting ID
        //    using (var document = JsonDocument.Parse(responseContent))
        //    {
        //        var root = document.RootElement;
        //        if (root.TryGetProperty("id", out var meetingIdElement))
        //        {
        //            meetingId = meetingIdElement.GetString() ?? string.Empty;
        //        }
        //    }

        //    // Return the meeting response with the meeting ID
        //    return new MeetingResponse { MeetingId = meetingId ?? string.Empty };
        //}


        ///// <summary>
        ///// Creates a Microsoft Teams meeting using the provided meeting details and sends a POST request to the Microsoft Graph API to schedule the event.
        ///// </summary>
        ///// <param name="requestMeeting"></param>
        ///// <param name="currentUserEmail"></param>
        ///// <returns></returns>
        //private async Task<HttpResponseMessage> CreateMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        //{
        //    // Construct the meeting content with the create details
        //    var meetingcontent = BuildMeetingContent(requestMeeting);

        //    // Set the endpoint URL for Microsoft Graph API to create events
        //    string endpoint = $"/v1.0/users/{currentUserEmail}/events";

        //    // Send the HTTP POST request to create the meeting
        //    return await _httpClient.PostAsync(endpoint, meetingcontent);
        //}


        ///// <summary>
        ///// Updates an existing Microsoft Teams meeting using the provided meeting details and sends a PATCH request to the Microsoft Graph API.
        ///// </summary>
        ///// <param name="requestMeeting"></param>
        ///// <param name="currentUserEmail"></param>
        ///// <returns></returns>
        //private async Task<HttpResponseMessage> UpdateMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        //{
        //    // Check if the MeetingId is null or empty in the request. 
        //    // If it is, return a BadRequest (400) response indicating that the request is invalid.
        //    if (string.IsNullOrEmpty(requestMeeting.MeetingId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);

        //    // Construct the event object with the updated details
        //    var meetingcontent = BuildMeetingContent(requestMeeting);

        //    // Set the endpoint URL for Microsoft Graph API to create events
        //    string endpoint = $"/v1.0/users/{currentUserEmail}/events/{requestMeeting.MeetingId}";

        //    // Send the PATCH request to update the meeting
        //    return await _httpClient.PatchAsync(endpoint, meetingcontent);
        //}


        ///// <summary>
        ///// Deletes an existing Microsoft Teams meeting by sending a DELETE request to the Microsoft Graph API.
        ///// </summary>
        ///// <param name="requestMeeting"></param>
        ///// <param name="currentUserEmail"></param>
        ///// <returns></returns>
        //private async Task<HttpResponseMessage> DeleteMeeting(MeetingRequest requestMeeting, string currentUserEmail)
        //{
        //    // Check if the MeetingId is null or empty in the request. 
        //    // If it is, return a BadRequest (400) response indicating that the request is invalid.
        //    if (string.IsNullOrEmpty(requestMeeting.MeetingId))
        //        return new HttpResponseMessage(HttpStatusCode.BadRequest);

        //    // Set the endpoint URL for Microsoft Graph API to create events
        //    string endpoint = $"/v1.0/users/{currentUserEmail}/events/{requestMeeting.MeetingId}";

        //    // Delete the event by sending a DELETE request to the Microsoft Graph API
        //    return await _httpClient.DeleteAsync(endpoint);
        //}



        #region Private Methods

        #region Helper Methods
        /// <summary>
        /// Builds the recurrence pattern based on the provided recurrence details.
        /// </summary>
        /// <param name="recurrence"></param>
        /// <returns></returns>
        private object BuildReccurencePattern(RecurrencePattern recurrence)
        {
            // Get the appropriate recurrence builder based on the recurrence pattern type
            var builder = _recurrencePatternFactory.GetRecurrenceBuilder(recurrence.ReccurrencePatternType);

            // Build the recurrence pattern using the builder
            return builder.BuildRecurrencePattern(recurrence);
        }

        /// <summary>
        /// Builds the meeting data from the meeting request details.
        /// </summary>
        private StringContent BuildMeetingContent(MeetingRequest requestMeeting)
        {
            // Build recurrence pattern if the meeting is recurring
            object? recurrencePattern = null;

            // Check if the meeting is recurring and has recurrence details
            if (requestMeeting.IsReccurring && requestMeeting.Reccurrence != null)
            {
                // Build the recurrence pattern using the builder
                recurrencePattern = BuildReccurencePattern(requestMeeting.Reccurrence);
            }

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
                OnlineMeetingProvider = "TeamsForBusiness",
                Recurrence = recurrencePattern
            };

            // Serialize event data to JSON
            return meetingData.ToJsonContent();
        }

        /// <summary>
        /// Extracts meeting details from the HTTP response after creating a meeting.
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        private static async Task<MeetingResponse> ExtractCreatedMeetingDetailsResponseAsync(HttpResponseMessage response)
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
            return new MeetingResponse
            {
                IsSuccess = true,
                MeetingResponseType = EnumMeetingResponseType.MeetingCreated,
                MeetingId = meetingId ?? string.Empty
            };
        }

        #endregion

        #region Meeting Action Handlers
        /// <summary>
        /// Handles the event action (Create, Update, Delete) based on the meeting request.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        private async Task<MeetingResponse> HandleEventAction(MeetingRequest requestMeeting, string currentUserEmail)
        {
            return requestMeeting.MeetingAction switch
            {
                EnumMeetingAction.CreateNewEvent =>
                            await CreateMeetingAsync(requestMeeting, currentUserEmail),
                EnumMeetingAction.UpdateEvent =>
                            await UpdateMeetingActionAsync(requestMeeting, currentUserEmail),
                EnumMeetingAction.DeleteEvent =>
                            await DeleteMeetingActionAsync(requestMeeting, currentUserEmail),
                _ =>
                    throw new NotSupportedException($"Action {requestMeeting.MeetingAction} is not supported.")
            };
        }

        #endregion

        #region Create Meeting Methods

        /// <summary>
        /// Creates a Microsoft Teams meeting using the provided meeting details and sends a POST request to the Microsoft Graph API to schedule the event.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> CreateMeetingAsync(MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Construct the meeting content with the create details
            var meetingcontent = BuildMeetingContent(requestMeeting);

            // Set the endpoint URL for Microsoft Graph API to create events
            string endpoint = $"/v1.0/users/{currentUserEmail}/events";

            // Send the POST request to create the meeting
            HttpResponseMessage response = await _httpClient.PostAsync(endpoint, meetingcontent);

            // Handle unsuccessful response
            if (!response.IsSuccessStatusCode)
            {
                var ex = new HttpRequestException($"Failed to create meeting. Status Code: {response.StatusCode} , Reason: {response.ReasonPhrase}");
               Console.WriteLine(ex);
                throw ex;
            }

            // Extract meeting details from the response
            MeetingResponse meetingResponse = await ExtractCreatedMeetingDetailsResponseAsync(response);

            // If the meeting is recurring, retrieve the meeting instances
            if (requestMeeting.IsReccurring)
            {
                // Get the recurrence start and end dates
                DateTime startDateTime = requestMeeting.Reccurrence!.ReccurrenceStartDate;
                DateTime endDateTime = requestMeeting.Reccurrence.ReccurrenceEndDate;

                // Retrieve meeting instances within the specified date range
                meetingResponse.MeetingInstances = await GetMeetingInstancesAsync(meetingResponse.MeetingId, currentUserEmail, startDateTime, endDateTime);

                // Set the meeting response type to indicate a recurring meeting was created
                meetingResponse.MeetingResponseType = EnumMeetingResponseType.RecurringMeetingCreated;
            }

            // Return the meeting response
            return meetingResponse;
        }

        #endregion

        #region Update Meeting Methods

        /// <summary>
        /// Updates an existing Microsoft Teams meeting using the provided meeting details and sends a PATCH request to the Microsoft Graph API.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> UpdateMeetingActionAsync(MeetingRequest requestMeeting, string currentUserEmail)
        {
            #region Input Validation

            // Validate meetingId
            if (string.IsNullOrWhiteSpace(requestMeeting.MeetingId))
                throw new ArgumentException("Meeting ID must be provided for updating a meeting.", nameof(requestMeeting.MeetingId));

            // Validate meeting instance ID for recurring meetings
            if (requestMeeting.IsReccurring && string.IsNullOrWhiteSpace(requestMeeting.MeetingInstanceId))
                throw new ArgumentException("Meeting instance ID must be provided for updating a specific occurrence of a recurring meeting.", nameof(requestMeeting.MeetingInstanceId));

            #endregion

            // Determine the update scope and call the appropriate update method
            return requestMeeting.IsReccurring switch
            {
                true when requestMeeting.Reccurrence!.RecureneceEditMode == EnumRecureneceEditMode.ThisOccurrence =>
                                    await UpdateSingleMeetingAsync(requestMeeting.MeetingInstanceId!, requestMeeting, currentUserEmail, EnumMeetingResponseType.RecurringMeetingUpdated),

                true when requestMeeting.Reccurrence!.RecureneceEditMode == EnumRecureneceEditMode.ThisAndFuture =>
                                    await UpdateThisAndFutureMeetingsAsync(requestMeeting.MeetingId, requestMeeting, currentUserEmail),

                _ => await UpdateSingleMeetingAsync(requestMeeting.MeetingId, requestMeeting, currentUserEmail, EnumMeetingResponseType.MeetingUpdated)
            };
        }

        /// <summary>
        /// Updates a single Microsoft Teams meeting using the provided meeting ID and content, and sends a PATCH request to the Microsoft Graph API.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="meetingcontent"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> UpdateMeetingAsync(string meetingId, StringContent meetingcontent, string currentUserEmail, EnumMeetingResponseType responseType)
        {
            // Set the endpoint URL for Microsoft Graph API to update events
            string endpoint = $"/v1.0/users/{currentUserEmail}/events/{meetingId}";

            // Send the PATCH request to update the meeting
            HttpResponseMessage response = await _httpClient.PatchAsync(endpoint, meetingcontent);

            // Handle unsuccessful response
            if (!response.IsSuccessStatusCode)
            {
                var ex = new HttpRequestException($"Failed to update meeting. Status Code: {response.StatusCode} , Reason: {response.ReasonPhrase}");
                Console.WriteLine(ex);
                throw ex;
            }

            // Return the meeting response with the updated meeting ID
            return new()
            {
                IsSuccess = true,
                MeetingResponseType = responseType,
                MeetingId = meetingId
            };
        }

        /// <summary>
        /// Updates a single Microsoft Teams meeting using the provided meeting ID and request details.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> UpdateSingleMeetingAsync(string meetingId, MeetingRequest requestMeeting, string currentUserEmail, EnumMeetingResponseType responseType) =>
            await UpdateMeetingAsync(meetingId, BuildMeetingContent(requestMeeting), currentUserEmail, responseType);

        /// <summary>
        /// Updates a Microsoft Teams meeting and all future occurrences using the provided meeting ID and request details.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> UpdateThisAndFutureMeetingsAsync(string meetingId, MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Retrieve the existing meeting details
            var meetingDetails = await GetMeetingAsync(meetingId, currentUserEmail);

            // Modify the recurrence pattern to end before the updated occurrence
            var recurrence = meetingDetails.Recurrence!;

            // Check if recurrence split is required
            bool isRecurrenceSplitRequired = await IsRecurrenceSplitRequiredAsync(meetingId, currentUserEmail, recurrence.Range.StartDate.Date, recurrence.Range.EndDate.Date, requestMeeting.Reccurrence!.ReccurrenceStartDate.Date);

            // Prepare the updated meeting content
            StringContent updatedMeetingContent;

            // If recurrence split is required, update the existing meeting and create a new one
            if (isRecurrenceSplitRequired)
            {
                // Build the updated meeting content
                updatedMeetingContent = BuildRecurrenceMeetingContent(recurrence, requestMeeting.Reccurrence.ReccurrenceStartDate.AddDays(-1));

                // Update the existing meeting to truncate the recurrence
                await UpdateMeetingAsync(meetingId, updatedMeetingContent, currentUserEmail, EnumMeetingResponseType.RecurringMeetingUpdated);

                // Create a new meeting for the updated occurrence and future meetings
                return await CreateMeetingAsync(requestMeeting, currentUserEmail);
            }

            // No recurrence split required, update the existing meeting
            updatedMeetingContent = BuildMeetingContent(requestMeeting);

            // Update the existing meeting
            var meetingResponse = await UpdateMeetingAsync(meetingId, updatedMeetingContent, currentUserEmail, EnumMeetingResponseType.RecurringMeetingUpdated);

            // If the meeting is recurring, retrieve the meeting instances
            if (requestMeeting.IsReccurring)
            {
                // Get the recurrence start and end dates
                DateTime startDateTime = requestMeeting.Reccurrence!.ReccurrenceStartDate;
                DateTime endDateTime = requestMeeting.Reccurrence.ReccurrenceEndDate;

                // Retrieve meeting instances within the specified date range
                meetingResponse.MeetingInstances = await GetMeetingInstancesAsync(meetingResponse.MeetingId, currentUserEmail, startDateTime, endDateTime);

                // Set the meeting response type to indicate a recurring meeting was updated
                meetingResponse.MeetingResponseType = EnumMeetingResponseType.RecurringMeetingUpdated;
            }

            return meetingResponse;

        }

        /// <summary>
        /// Builds the recurrence meeting content for splitting the recurrence pattern.
        /// </summary>
        /// <param name="recurrence"></param>
        /// <param name="recurrenceEndDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static StringContent BuildRecurrenceMeetingContent(RecurrenceResponse recurrence, DateTime recurrenceEndDate)
        {
            // Validate recurrence pattern
            if (recurrence.Range == null)
                throw new ArgumentException("Recurrence range cannot be null.", nameof(recurrence));

            // recurrence pattern object based on the type
            object patten = recurrence.Pattern.Type switch
            {
                "daily" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval
                },
                "weekly" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval,
                    recurrence.Pattern.DaysOfWeek,
                },
                "absoluteMonthly" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval,
                    recurrence.Pattern.DayOfMonth,
                },
                "relativeMonthly" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval,
                    recurrence.Pattern.DaysOfWeek,
                    recurrence.Pattern.Index,
                    recurrence.Pattern.FirstDayOfWeek
                },
                "absoluteYearly" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval,
                    recurrence.Pattern.Month,
                    recurrence.Pattern.DayOfMonth,
                },
                "relativeYearly" => new
                {
                    recurrence.Pattern.Type,
                    recurrence.Pattern.Interval,
                    recurrence.Pattern.Month,
                    recurrence.Pattern.DaysOfWeek,
                    recurrence.Pattern.Index,
                    recurrence.Pattern.FirstDayOfWeek
                },
                _ => throw new ArgumentException($"Unsupported recurrence pattern type: {recurrence.Pattern.Type}", nameof(recurrence))
            };

            //  recurrence range object
            var updatedRecurrence = new
            {
                Recurrence = new
                {
                    Pattern = patten,
                    Range = new
                    {
                        recurrence.Range.Type,
                        startDate = recurrence.Range.StartDate.ToString("yyyy-MM-dd"),
                        endDate = recurrenceEndDate.ToString("yyyy-MM-dd"),
                        recurrenceTimeZone = recurrence.Range.RecurrenceTimeZone,
                        numberOfOccurrences = recurrence.Range.NumberOfOccurrences
                    }
                }
            };

            // Serialize the updated recurrence object to JSON content
            return updatedRecurrence.ToJsonContent();
        }

        #endregion

        #region Delete Meeting Methods

        /// <summary>
        /// Deletes a Microsoft Teams meeting based on the provided meeting request and user email.
        /// </summary>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<MeetingResponse> DeleteMeetingActionAsync(MeetingRequest requestMeeting, string currentUserEmail)
        {
            #region Input Validation

            // Validate meetingId
            if (string.IsNullOrWhiteSpace(requestMeeting.MeetingId))
                throw new ArgumentException("Meeting ID must be provided for updating a meeting.", nameof(requestMeeting.MeetingId));

            // Validate meeting instance ID for recurring meetings
            if (requestMeeting.IsReccurring && requestMeeting.Reccurrence!.RecureneceEditMode == EnumRecureneceEditMode.ThisOccurrence && string.IsNullOrWhiteSpace(requestMeeting.MeetingInstanceId))
                throw new ArgumentException("Meeting instance ID must be provided for updating a specific occurrence of a recurring meeting.", nameof(requestMeeting.MeetingInstanceId));

            #endregion

            // Determine the update scope and call the appropriate update method
            return requestMeeting.IsReccurring switch
            {
                true when requestMeeting.Reccurrence!.RecureneceEditMode == EnumRecureneceEditMode.ThisOccurrence =>
                                    await DeleteMeetingAsync(requestMeeting.MeetingInstanceId!, currentUserEmail, EnumMeetingResponseType.RecurringMeetingDeleted),

                true when requestMeeting.Reccurrence!.RecureneceEditMode == EnumRecureneceEditMode.ThisAndFuture =>
                                    await DeleteThisAndFutureMeetingsAsync(requestMeeting.MeetingId, requestMeeting, currentUserEmail),

                _ => await DeleteMeetingAsync(requestMeeting.MeetingId, currentUserEmail, EnumMeetingResponseType.MeetingDeleted)
            };
        }

        /// <summary>
        /// Deletes a single Microsoft Teams meeting using the provided meeting ID and sends a DELETE request to the Microsoft Graph API.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<MeetingResponse> DeleteMeetingAsync(string meetingId, string currentUserEmail, EnumMeetingResponseType responseType)
        {
            #region Input Validation
            // Validate meetingId
            if (string.IsNullOrWhiteSpace(meetingId))
                throw new ArgumentException("Meeting ID must be provided for updating a meeting.", nameof(meetingId));
            #endregion

            // Construct the endpoint URL for deleting the event
            string endpoint = $"/v1.0/users/{currentUserEmail}/events/{meetingId}";

            // Send the DELETE request to delete the meeting
            HttpResponseMessage response = await _httpClient.DeleteAsync(endpoint);

            // Handle unsuccessful response
            if (!response.IsSuccessStatusCode)
            {
                var ex = new HttpRequestException($"Failed to delete meeting. Status Code: {response.StatusCode} , Reason: {response.ReasonPhrase}");
                Console.WriteLine(ex);
                throw ex;
            }

            // Return the meeting response with the deleted meeting ID
            return new()
            {
                IsSuccess = true,
                MeetingResponseType = responseType,
                MeetingId = meetingId
            };
        }

        /// <summary>
        /// Deletes a Microsoft Teams meeting and all future occurrences using the provided meeting ID and request details.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="requestMeeting"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        private async Task<MeetingResponse> DeleteThisAndFutureMeetingsAsync(string meetingId, MeetingRequest requestMeeting, string currentUserEmail)
        {
            // Retrieve the existing meeting details
            var meetingDetails = await GetMeetingAsync(meetingId, currentUserEmail);

            // Modify the recurrence pattern to end before the updated occurrence
            var recurrence = meetingDetails.Recurrence!;

            // Check if recurrence split is required
            bool isRecurrenceSplitRequired = await IsRecurrenceSplitRequiredAsync(meetingId, currentUserEmail, recurrence.Range.StartDate.Date, recurrence.Range.EndDate.Date, requestMeeting.Reccurrence!.ReccurrenceStartDate.Date);

            // Update the existing meeting to truncate the recurrence if required
            if (isRecurrenceSplitRequired)
            {
                // Build the updated meeting content
                StringContent updatedMeetingContent = BuildRecurrenceMeetingContent(recurrence, requestMeeting.Reccurrence.ReccurrenceStartDate.AddDays(-1));

                // Update the existing meeting to truncate the recurrence
                return await UpdateMeetingAsync(meetingId, updatedMeetingContent, currentUserEmail, EnumMeetingResponseType.RecurringMeetingUpdated);

                // Delete the specific meeting instance
                //return await DeleteMeetingAsync(instanceId, currentUserEmail, EnumMeetingResponseType.RecurringMeetingDeleted);
            }

            // Delete the entire meeting series
            return await DeleteMeetingAsync(meetingId, currentUserEmail, EnumMeetingResponseType.MeetingDeleted);

        }

        #endregion

        #region Get Meeting Methods

        /// <summary>
        /// Check must split recurrence meeting 
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="currentUserEmail"></param>
        /// <param name="startDateTime"></param>
        /// <param name="endDateTime"></param>
        /// <param name="recurrenceDateTime"></param>
        /// <returns></returns>
        private async Task<bool> IsRecurrenceSplitRequiredAsync(string meetingId, string currentUserEmail, DateTime startDateTime, DateTime endDateTime, DateTime recurrenceDateTime)
        {
            // Retrieve meeting instances within the specified date range
            var instances = await GetMeetingInstancesAsync(meetingId, currentUserEmail, startDateTime, endDateTime.AddDays(1).AddTicks(-1));

            // early return no instances
            if (instances == null || !instances.Any())
                return false;

            // get first meeting
            var firstInstance = instances.Where(x => !string.Equals(x.Type, "deleted", StringComparison.OrdinalIgnoreCase))
                                    .OrderBy(x => x.Start.DateTime.Date)
                                    .FirstOrDefault();

            // return false if empty date
            if (firstInstance?.Start?.DateTime == null)
                return false;

            // fist instance date
            var firstInstanceStartDate = firstInstance.Start.DateTime.Date;

            // check first instance date and recurence start date is same
            return firstInstanceStartDate != recurrenceDateTime.Date;
        }



        /// <summary>
        /// Retrieves a specific meeting by sending a GET request to the Microsoft Graph API.
        /// </summary>
        /// <param name="meetingId"></param>
        /// <param name="currentUserEmail"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<MeetingInstanceResponse> GetMeetingAsync(string meetingId, string currentUserEmail)
        {
            #region Input Validation
            // Validate current user email address
            if (string.IsNullOrWhiteSpace(currentUserEmail))
                throw new ArgumentException("User email must be provided.", nameof(currentUserEmail));

            // Validate meetingId
            if (string.IsNullOrWhiteSpace(meetingId))
                throw new ArgumentException("Meeting ID must be provided.", nameof(meetingId));
            #endregion 

            // Construct the endpoint URL for retrieving event instances
            string endpoint = $@"/v1.0/users/{currentUserEmail}/events/{meetingId}";

            try
            {
                // Send GET request to retrieve event instances
                var response = await _httpClient.GetAsync(endpoint);

                // Handle unsuccessful response
                if (!response.IsSuccessStatusCode)
                {
                    var ex = new HttpRequestException($"Failed to retrieve meeting details. Status Code: {response.StatusCode} , Reason: {response.ReasonPhrase}");
                    Console.WriteLine(ex);
                    throw ex;
                }

                // Read response content as string
                var json = await response.Content.ReadAsStringAsync();

                // Set up JSON serializer options for case-insensitive property matching
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                // Deserialize the JSON response to get the meeting instance
                return JsonSerializer.Deserialize<MeetingInstanceResponse>(json, options) ?? new();
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error retrieving meeting instances.{e}");
                throw;
            }
        }


        /// <summary>
        /// Retrieves instances of a recurring meeting within a specified date range by sending a GET request to the Microsoft Graph API.
        /// </summary>
        /// <param name="currentUserEmail"></param>
        /// <param name="meetingId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private async Task<List<MeetingInstanceResponse>> GetMeetingInstancesAsync(string meetingId, string currentUserEmail, DateTime startDateTime, DateTime endDateTime)
        {
            #region Input Validation
            // Validate current user email address
            if (string.IsNullOrWhiteSpace(currentUserEmail))
                throw new ArgumentException("User email must be provided.", nameof(currentUserEmail));

            // Validate meetingId
            if (string.IsNullOrWhiteSpace(meetingId))
                throw new ArgumentException("Meeting ID must be provided.", nameof(meetingId));

            // Validate start and end dates
            if (startDateTime == default)
                throw new ArgumentException("Start date must be provided.", nameof(startDateTime));

            // Validate end date
            if (endDateTime == default)
                throw new ArgumentException("End date must be provided.", nameof(endDateTime));

            // Validate that start date is earlier than end date
            if (startDateTime >= endDateTime)
                throw new ArgumentException("Start date must be earlier than end date.");

            // Validate that end date is later than start date
            if (endDateTime <= startDateTime)
                throw new ArgumentException("End date must be later than start date.");

            #endregion 


            List<MeetingInstanceResponse> meetingInstances = new();

            // Construct the endpoint URL for retrieving event instances
            string endpoint = $@"/v1.0/users/{currentUserEmail}/events/{meetingId}/instances?startDateTime={startDateTime:yyyy-MM-ddTHH:mm:ss}&endDateTime={endDateTime:yyyy-MM-ddTHH:mm:ss}";

            try
            {
                string nextLink = endpoint;

                while (!string.IsNullOrEmpty(nextLink))
                {
                    // Send GET request to retrieve event instances
                    var response = await _httpClient.GetAsync(nextLink);

                    // Ensure the response is successful
                    response.EnsureSuccessStatusCode();

                    // Read response content as string
                    var json = await response.Content.ReadAsStringAsync();

                    // Set up JSON serializer options for case-insensitive property matching
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    // Deserialize the JSON response to get the list of meeting instances
                    var result = JsonSerializer.Deserialize<InstancesResponse>(json, options);

                    // Return the list of meeting instances if available
                    if (result?.Value is { Count: > 0 })
                    {
                        meetingInstances.AddRange(result.Value);
                    }

                    // Get the next link for pagination
                    nextLink = result?.NextLink ?? string.Empty;

                    // Adjust nextLink if it is a full URL
                    if (!string.IsNullOrEmpty(nextLink) && nextLink.StartsWith("https"))
                    {
                        Uri uri = new(nextLink);
                        nextLink = uri.PathAndQuery;
                    }
                }

                // Return the list of meeting instances
                return meetingInstances;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unexpected error retrieving meeting instances.{e}");
                throw;
            }
        }
        #endregion

        #endregion

    }
}
