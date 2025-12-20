namespace StuffyHelper.Minio.Features.Storage
{
    /// <summary>
    /// File storage interface
    /// </summary>
    public interface IFileStore
    {
        /// <summary>
        /// Delete file if exists
        /// </summary>
        /// <param name="objectName">File identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteFilesIfExistAsync(
            string objectName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get file
        /// </summary>
        /// <param name="objectName">File identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<Stream> GetFileAsync(
            string objectName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Upload file to store
        /// </summary>
        /// <param name="stream">File stream</param>
        /// <param name="objectName">File identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task StoreFileAsync(
            string objectName,
            Stream stream,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get presigned url
        /// </summary>
        /// <param name="objectName">File identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<Uri> ObtainGetPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Put presigned url
        /// </summary>
        /// <param name="objectName">File identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task<Uri> ObtainPutPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default);
    }
}
