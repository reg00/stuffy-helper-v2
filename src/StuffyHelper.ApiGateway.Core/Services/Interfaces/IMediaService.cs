using StuffyHelper.Common.Client;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with media files in API Gateway
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// Delete media by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteMediaAsync(string token, Guid eventId, Guid mediaId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get media file as blob with metadata
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Media file parameter with data and metadata</returns>
        Task<FileParam> GetMediaFormFileAsync(string token, Guid eventId, Guid mediaId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get media metadata by identifier
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed media metadata</returns>
        Task<GetMediaEntry> GetMediaMetadataAsync(string token, Guid eventId, Guid mediaId, CancellationToken cancellationToken = default);

        /// <summary>
        /// Get filtered list of media metadata
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="createdDateStart">Minimum creation date filter</param>
        /// <param name="createdDateEnd">Maximum creation date filter</param>
        /// <param name="mediaType">Media type filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of media metadata</returns>
        Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            string token,
            Guid eventId,
            int offset,
            int limit,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Store media file from form data
        /// </summary>
        /// <param name="token">Authentication token</param>
        /// <param name="eventId">Event id</param>
        /// <param name="media">Media data to store</param>
        /// <param name="isPrimal">Indicates if this is primary media for event</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Stored media metadata</returns>
        Task<MediaShortEntry> StoreMediaFormFileAsync(
            string token,
            Guid eventId,
            AddMediaEntry media,
            bool isPrimal = false,
            CancellationToken cancellationToken = default);
    }
}