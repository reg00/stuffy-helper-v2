using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IMediaClient" />
public class MediaClient: ApiClientBase, IMediaClient
{
    private const string DefaultRoute = "api/v1/media";
    
    /// <summary>
    /// Ctor.
    /// </summary>
    /// <param name="baseUrl"></param>
    public MediaClient(string baseUrl) : base(baseUrl)
    {
        
    }

    /// <inheritdoc />
    public Task<MediaShortEntry> StoreMediaFormFileAsync(
        string token,
        AddMediaEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/form-file")
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<MediaShortEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{mediaId}/form-file")
            .AddBearerToken(token);

        return GetFile(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{mediaId}/metadata")
            .AddBearerToken(token);

        return Get<GetMediaEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteMediaAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/{mediaId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        DateTimeOffset? createdDateStart = null,
        DateTimeOffset? createdDateEnd = null,
        MediaType? mediaType = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/metadata")
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(eventId), eventId)
            .AddOptionalQueryParameter(nameof(createdDateStart), createdDateStart)
            .AddOptionalQueryParameter(nameof(createdDateEnd), createdDateEnd)
            .AddOptionalQueryParameter(nameof(mediaType), mediaType);

        return Get<IEnumerable<MediaShortEntry>>(request, cancellationToken);
    }
}