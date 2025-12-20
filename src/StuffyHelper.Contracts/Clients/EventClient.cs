using Microsoft.AspNetCore.Http;
using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Client.Helpers;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IEventClient" />
public class EventClient: ApiClientBase, IEventClient
{
    private const string DefaultRoute = "api/v1/events";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public EventClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    /// <inheritdoc />
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
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(name), name)
            .AddOptionalQueryParameter(nameof(description), description)
            .AddOptionalQueryParameter(nameof(createdDateStart), createdDateStart)
            .AddOptionalQueryParameter(nameof(createdDateEnd), createdDateEnd)
            .AddOptionalQueryParameter(nameof(eventDateStartMin), eventDateStartMin)
            .AddOptionalQueryParameter(nameof(eventDateStartMax), eventDateStartMax)
            .AddOptionalQueryParameter(nameof(eventDateEndMin), eventDateEndMin)
            .AddOptionalQueryParameter(nameof(eventDateEndMax), eventDateEndMax)
            .AddOptionalQueryParameter(nameof(userId), userId)
            .AddOptionalQueryParameter(nameof(isCompleted), isCompleted)
            .AddOptionalQueryParameter(nameof(isActive), isActive)
            .AddOptionalQueryParameter(nameof(participantId), participantId)
            .AddOptionalQueryParameter(nameof(purchaseId), purchaseId);

        return Get<Response<EventShortEntry>>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<GetEventEntry> GetEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}")
            .AddBearerToken(token);

        return Get<GetEventEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<EventShortEntry> CreateEventAsync(
        string token,
        AddEventEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(DefaultRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<EventShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteEventAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<EventShortEntry> UpdateEventAsync(
        string token,
        Guid eventId,
        UpdateEventEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Patch<EventShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteEventAvatarAsync(
        string token,
        Guid eventId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}/photo")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<EventShortEntry> UpdateEventAvatarAsync(
        string token,
        Guid eventId,
        IFormFile file,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}/photo")
            .AddBearerToken(token)
            .AddFile("file", file.ToFileParam());

        return Patch<EventShortEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}/complete")
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}/reopen")
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{eventId}/checkout")
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
}