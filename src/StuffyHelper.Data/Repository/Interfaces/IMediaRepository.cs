using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with media repository
    /// </summary>
    public interface IMediaRepository
    {
        /// <summary>
        /// Add new media
        /// </summary>
        /// <param name="media">Media data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created media entry</returns>
        Task<MediaEntry> AddMediaAsync(
            MediaEntry media,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get media by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="mediaId">Media identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Media entry</returns>
        Task<MediaEntry> GetMediaAsync(
            Guid eventId, 
            Guid mediaId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get primary media for event
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Primary media entry or null if not found</returns>
        Task<MediaEntry?> GetPrimalEventMedia(
            Guid eventId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get filtered list of media
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="createdDateStart">Minimum creation date filter</param>
        /// <param name="createdDateEnd">Maximum creation date filter</param>
        /// <param name="mediaType">Media type filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of media entries</returns>
        Task<IEnumerable<MediaEntry>> GetMediasAsync(
            Guid eventId,
            int offset,
            int limit,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete media
        /// </summary>
        /// <param name="media">Media entry to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteMediaAsync(
            MediaEntry media,
            CancellationToken cancellationToken = default);
    }
}