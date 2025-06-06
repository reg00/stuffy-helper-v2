using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface IMediaRepository
    {
        Task<MediaEntry> AddMediaAsync(
            MediaEntry media,
            CancellationToken cancellationToken = default);

        Task<MediaEntry> GetMediaAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        Task<MediaEntry> GetPrimalEventMedia(
            Guid eventId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MediaEntry>> GetMediasAsync(
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        Task DeleteMediaAsync(
            MediaEntry media,
            CancellationToken cancellationToken = default);
    }
}
