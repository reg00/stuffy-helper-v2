using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IParticipantClient" />
public class ParticipantClient: ApiClientBase, IParticipantClient
{
    /// <summary>
    /// Ctor.
    /// </summary>
    public ParticipantClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        string? userId = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetParticipantsRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(eventId), eventId)
            .AddOptionalQueryParameter(nameof(userId), userId);

        return Get<Response<ParticipantShortEntry>>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<GetParticipantEntry> GetParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetParticipantRoute)
            .AddBearerToken(token);

        return Get<GetParticipantEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<ParticipantShortEntry> CreateParticipantAsync(
        string token,
        UpsertParticipantEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.AddParticipantRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<ParticipantShortEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeleteParticipantRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
}