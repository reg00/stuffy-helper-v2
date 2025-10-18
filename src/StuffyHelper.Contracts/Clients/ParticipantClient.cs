using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IParticipantClient" />
public class ParticipantClient: ApiClientBase, IParticipantClient
{
    private const string DefaultRoute = "api/v1";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    public ParticipantClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
        string token,
        Guid eventId,
        int offset = 0,
        int limit = 10,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/participants")
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(userId), userId);

        return Get<Response<ParticipantShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetParticipantEntry> GetParticipantAsync(
        string token,
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/participants/{participantId}")
            .AddBearerToken(token);

        return Get<GetParticipantEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<ParticipantShortEntry> CreateParticipantAsync(
        string token,
        Guid eventId,
        UpsertParticipantEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/participants")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<ParticipantShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteParticipantAsync(
        string token,
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/participants/{participantId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
}