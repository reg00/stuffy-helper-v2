using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with events
/// </summary>
public interface IEventClient
{
    /// <summary>
    /// Return lit of events
    /// </summary>
    public Task<Response<EventShortEntry>> GetEventsAsync(
        string token,
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
    /// Return event by id
    /// </summary>
    public Task<GetEventEntry> GetEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new event
    /// </summary>
    public Task<EventShortEntry> CreateEventAsync(
        string token,
        AddEventEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Soft delete event
    /// </summary>
    public Task DeleteEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update event data
    /// </summary>
    public Task<EventShortEntry> UpdateEventAsync(
        string token,
        Guid eventId,
        UpdateEventEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove event avatar
    /// </summary>
    public Task DeleteEventAvatarAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update event avatar
    /// </summary>
    public Task<EventShortEntry> UpdateEventAvatarAsync(
        string token,
        Guid eventId,
        IFormFile file,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Complete event
    /// </summary>
    public Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reopen event
    /// </summary>
    public Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Checkout event
    /// </summary>
    public Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);
}