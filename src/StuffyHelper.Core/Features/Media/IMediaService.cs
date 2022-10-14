namespace StuffyHelper.Core.Features.Media
{
    public interface IMediaService
    {
        Task<MediaBlobEntry> GetMediaFormFileAsync(
            Guid eventId,
            string mediaUid,
            CancellationToken cancellationToken = default);

        Task<GetMediaEntry> GetMediaMetadataAsync(
            Guid eventId,
            string mediaUid,
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
            Guid eventId,
            string mediaUid,
            CancellationToken cancellationToken = default);

        Task<MediaShortEntry> StoreMediaFormFileAsync(
            Guid eventId,
            string mediaUid,
            FileType fileType,
            Stream requestStream,
            MediaType mediaType,
            string link = null,
            bool? isPrimal = null,
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
