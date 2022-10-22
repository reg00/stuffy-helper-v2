namespace StuffyHelper.Core.Features.Media
{
    public interface IMediaService
    {
        Task<MediaBlobEntry> GetMediaFormFileAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        Task<GetMediaEntry> GetMediaMetadataAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default);

        Task DeleteMediaAsync(
            Guid mediaId,
            CancellationToken cancellationToken = default);

        Task<MediaShortEntry> StoreMediaFormFileAsync(
            AddMediaEntry mediaEntry,
            bool isPrimal = false,
            CancellationToken cancellationToken = default);

        Task<Uri> GetEventPrimalMediaUri(
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
