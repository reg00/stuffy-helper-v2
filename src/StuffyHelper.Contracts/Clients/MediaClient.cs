using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

public class MediaClient: ApiClientBase, IMediaClient
{
    public MediaClient(string baseUrl) : base(baseUrl)
    {
        
    }

    public Task<MediaShortEntry> StoreMediaFormFileAsync(
        string token,
        AddMediaEntry body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.StoreMediaFormFileRoute)
            .AddBearerToken(token)
            .AddJsonBody(body);

        return Post<MediaShortEntry>(request, cancellationToken);
    }

    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.RetrieveMediaFromFileRoute)
            .AddBearerToken(token);

        return GetFile(request, cancellationToken);
    }

    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetMediaMetadataRoute)
            .AddBearerToken(token);

        return Get<GetMediaEntry>(request, cancellationToken);
    }
    
    public Task DeleteMediaAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.DeleteMediaRoute)
            .AddBearerToken(token);

        return Delete(request, cancellationToken);
    }
    
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
        var request = CreateRequest(KnownRoutes.DeleteMediaRoute)
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