using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Client.Helpers;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.Contracts.Clients.Interface.IMediaClient" />
public class MediaClient: ApiClientBase, IMediaClient
{
    private const string DefaultRoute = "api/v1";
    
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
        Guid eventId,
        AddMediaEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/media/form-file")
            .AddBearerToken(token)
            .AddFile("file", body.File.ToFileParam())
            .AddParameter("mediaType", body.MediaType)
            .AddParameter("link", body.Link);

        return Post<MediaShortEntry>(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid eventId,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/media/{mediaId}/form-file")
            .AddBearerToken(token);

        return GetFile(request, cancellationToken);
    }

    /// <inheritdoc />
    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid eventId,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/media/{mediaId}/metadata")
            .AddBearerToken(token);

        return Get<GetMediaEntry>(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task DeleteMediaAsync(
        string token,
        Guid eventId,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/media/{mediaId}")
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
    /// <inheritdoc />
    public Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
        string token,
        Guid? eventId,
        int offset = 0,
        int limit = 10,
        DateTimeOffset? createdDateStart = null,
        DateTimeOffset? createdDateEnd = null,
        MediaType? mediaType = null,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/events/{eventId}/media/metadata")
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset)
            .AddOptionalQueryParameter(nameof(createdDateStart), createdDateStart)
            .AddOptionalQueryParameter(nameof(createdDateEnd), createdDateEnd)
            .AddOptionalQueryParameter(nameof(mediaType), mediaType);

        return Get<IEnumerable<MediaShortEntry>>(request, cancellationToken);
    }
}