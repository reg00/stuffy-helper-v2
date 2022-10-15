namespace StuffyHelper.Core.Features.Media
{
    public interface IMediaStore
    {
        Task<MediaEntry> AddMediaAsync(
            MediaEntry media,
            CancellationToken cancellationToken = default);

        Task<MediaEntry> GetMediaAsync(
            Guid mediaId,
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
