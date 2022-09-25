using StuffyHelper.Core.Features.Media;

namespace StuffyHelper.Core.Features.Common
{
    public interface IFileStore
    {
        Task DeleteFilesIfExistAsync(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default);

        Task<Stream> GetFileAsync(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default);

        Task StoreFileAsync(
            string eventId,
            string fileId,
            Stream stream,
            FileType fileType,
            CancellationToken cancellationToken = default);

        Task<Uri> ObtainGetPresignedUrl(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default);

        Task<Uri> ObtainPutPresignedUrl(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default);
    }
}
