using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with media
/// </summary>
public interface IMediaClient
{
    /// <summary>
    /// Add media for event
    /// </summary>
    public Task<MediaShortEntry> StoreMediaFormFileAsync(
        string token,
        AddMediaEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Download file by od
    /// </summary>
    public Task<FileParam> RetrieveMediaFormFileAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Get file metadata by id
    /// </summary>
    public Task<GetMediaEntry> GetMediaMetadataAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove media
    /// </summary>
    public Task DeleteMediaAsync(
        string token,
        Guid mediaId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Retuirn list of metadata
    /// </summary>
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