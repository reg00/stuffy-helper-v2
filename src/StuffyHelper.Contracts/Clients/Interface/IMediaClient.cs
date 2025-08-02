using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IMediaClient
{
    public Task<MediaShortEntry> StoreMediaFormFileAsync(
        string token,
        AddMediaEntry body,
        CancellationToken cancellationToken = default);

    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    public Task DeleteMediaAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    public Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        DateTimeOffset? createdDateStart = null,
        DateTimeOffset? createdDateEnd = null,
        MediaType? mediaType = null,
        CancellationToken cancellationToken = default);
}