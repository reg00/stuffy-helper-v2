using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IEventClient
{
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

    public Task<GetEventEntry> GetEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    public Task<EventShortEntry> CreateEventAsync(
        string token,
        AddEventEntry body,
        CancellationToken cancellationToken = default);

    public Task DeleteEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    public Task<EventShortEntry> UpdateEventAsync(
        string token,
        Guid eventId,
        UpdateEventEntry body,
        CancellationToken cancellationToken = default);

    public Task DeleteEventAvatarAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default);

    public Task<EventShortEntry> UpdateEventAvatarAsync(
        string token,
        Guid eventId,
        IFormFile file,
        CancellationToken cancellationToken = default);

    public Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    public Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

    public Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);
}