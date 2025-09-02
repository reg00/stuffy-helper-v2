using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with event repository
    /// </summary>
    public interface IEventRepository
    {
        /// <summary>
        /// Get event by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Event entry</returns>
        Task<EventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default);

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
        Task<Response<EventEntry>> GetEventsAsync(
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
        /// <param name="event">Event data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created event entry</returns>
        Task<EventEntry> AddEventAsync(EventEntry @event, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete event by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update event information
        /// </summary>
        /// <param name="event">Updated event data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated event entry</returns>
        Task<EventEntry> UpdateEventAsync(EventEntry @event, CancellationToken cancellationToken = default);
    }
}