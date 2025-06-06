namespace StuffyHelper.Minio.Features.Storage
{
    public interface IFileStore
    {
        Task DeleteFilesIfExistAsync(
            string objectName,
            CancellationToken cancellationToken = default);

        Task<Stream> GetFileAsync(
            string objectName,
            CancellationToken cancellationToken = default);

        Task StoreFileAsync(
            string objectName,
            Stream stream,
            CancellationToken cancellationToken = default);

        Task<Uri> ObtainGetPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default);

        Task<Uri> ObtainPutPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default);
    }
}
