using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with events
    /// </summary>
    public interface IEventService
    {
        /// <summary>
        /// Get event by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed event information</returns>
        Task<GetEventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get filtered list of events
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="name">Event name filter</param>
        /// <param name="description">Event description filter</param>
        /// <param name="createdDateStart">Minimum event creation date filter</param>
        /// <param name="createdDateEnd">Maximum event creation date filter</param>
        /// <param name="eventDateStartMin">Minimum event start date filter</param>
        /// <param name="eventDateStartMax">Maximum event start date filter</param>
        /// <param name="eventDateEndMin">Minimum event end date filter</param>
        /// <param name="eventDateEndMax">Maximum event end date filter</param>
        /// <param name="userId">User identifier filter (creator)</param>
        /// <param name="isCompleted">Event completion status filter</param>
        /// <param name="isActive">Event activity status filter</param>
        /// <param name="participantId">Participant identifier filter</param>
        /// <param name="purchaseId">Purchase identifier filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of events</returns>
        Task<Response<EventShortEntry>> GetEventsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            string? description = null,
            DateTime? createdDateStart = null,
            DateTime? createdDateEnd = null,
            DateTime? eventDateStartMin = null,
            DateTime? eventDateStartMax = null,
            DateTime? eventDateEndMin = null,
            DateTime? eventDateEndMax = null,
            string? userId = null,
            bool? isCompleted = null,
            bool? isActive = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new event
        /// </summary>
        /// <param name="eventEntry">Event data to add</param>
        /// <param name="claims">User claims for authorization</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created event information</returns>
        Task<EventShortEntry> AddEventAsync(
            AddEventEntry eventEntry,
            StuffyClaims claims,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete event by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteEventAsync(Guid eventId, string? userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update event information
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="updateEvent">Updated event data</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated event information</returns>
        Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry updateEvent, string? userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete primary event media
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeletePrimalEventMedia(Guid eventId, string? userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update primary event media
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="file">Media file to upload</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated event information</returns>
        Task<EventShortEntry> UpdatePrimalEventMediaAsync(Guid eventId, IFormFile file, string? userId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Mark event as completed or not completed
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="isComplete">Completion status</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated event information</returns>
        Task<EventShortEntry> CompleteEventAsync(Guid eventId, string? userId, bool isComplete, CancellationToken cancellationToken = default);
    }
}