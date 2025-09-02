using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with media files
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Get media file as blob with metadata
        /// </summary>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Media blob with file data and metadata</returns>
        Task<MediaBlobEntry> GetMediaFormFileAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get media metadata by identifier
        /// </summary>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed media metadata</returns>
        Task<GetMediaEntry> GetMediaMetadataAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get filtered list of media metadata
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="eventId">Event identifier filter</param>
        /// <param name="createdDateStart">Minimum creation date filter</param>
        /// <param name="createdDateEnd">Maximum creation date filter</param>
        /// <param name="mediaType">Media type filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of media metadata</returns>
        Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get primary media for event
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Primary media metadata or null if not found</returns>
        Task<GetMediaEntry?> GetPrimalEventMedia(
            Guid eventId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete media by identifier
        /// </summary>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteMediaAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Store media file from form data
        /// </summary>
        /// <param name="mediaEntry">Media data to store</param>
        /// <param name="isPrimal">Indicates if this is primary media for event</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Stored media metadata</returns>
        Task<MediaShortEntry> StoreMediaFormFileAsync(
            AddMediaEntry mediaEntry,
            bool isPrimal = false,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get URI for event's primary media
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Media URI or null if not found</returns>
        Task<Uri?> GetEventPrimalMediaUri(
            Guid eventId,
            CancellationToken cancellationToken = default);

        //Task<Uri> ObtainGetMediaPresignedUrl(
        //    Guid eventId,
        //    string mediaUid,
        //    CancellationToken cancellationToken = default);

        //Task<Uri> ObtainPutMediaPresignedUrl(
        //    Guid eventId,
        //    string mediaUid,
        //    FileType fileType,
        //    CancellationToken cancellationToken = default);
    }
}