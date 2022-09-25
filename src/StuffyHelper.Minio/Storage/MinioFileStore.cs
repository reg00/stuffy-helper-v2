using EnsureThat;
using Microsoft.Extensions.Options;
using Minio;
using Reg00.Infrastructure.Minio.Configs;
using Reg00.Infrastructure.Minio.Extensions;
using Reg00.Infrastructure.Minio.Features.Client;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Minio.Configs;
using System.Text;

namespace StuffyHelper.Minio.Storage
{
    public class MinioFileStore : IFileStore
    {
        private readonly MinioBucketClient _bucket;

        public MinioFileStore(MinioClient client,
            IOptionsMonitor<BlobContainerConfiguration> namedBlobBucketConfigurationAccessor,
            MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(client, nameof(client));
            EnsureArg.IsNotNull(namedBlobBucketConfigurationAccessor, nameof(namedBlobBucketConfigurationAccessor));

            var bucketConfiguration = namedBlobBucketConfigurationAccessor
                .Get(Constants.BlobBucketConfigurationName);

            _bucket = client.GetBucketClientAsync(minioClientOptions, bucketConfiguration.ContainerName).Result;
        }

        public MinioFileStore(MinioClient client,
            string bucketName,
            MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(client, nameof(client));
            EnsureArg.IsNotNull(bucketName, nameof(bucketName));

            _bucket = client.GetBucketClientAsync(minioClientOptions, bucketName).Result;
        }

        public async Task DeleteFilesIfExistAsync(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(eventId, nameof(eventId));
            EnsureArg.IsNotEmptyOrWhiteSpace(fileId, nameof(fileId));

            var objectName = GetObjectName(eventId, fileId, fileType);

            try
            {
                await _bucket.DeleteIfExistsAsync(objectName, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<Stream> GetFileAsync(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(eventId, nameof(eventId));
            EnsureArg.IsNotEmptyOrWhiteSpace(fileId, nameof(fileId));

            var objectName = GetObjectName(eventId, fileId, fileType);

            try
            {
                return await _bucket.DownloadAsync(objectName, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task StoreFileAsync(
            string eventId,
            string fileId,
            Stream stream,
            FileType fileType,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(eventId, nameof(eventId));
            EnsureArg.IsNotEmptyOrWhiteSpace(fileId, nameof(fileId));
            EnsureArg.IsNotNull(stream, nameof(stream));

            var objectName = GetObjectName(eventId, fileId, fileType);

            try
            {
                var memory = new Memory<byte>();
                await stream.ReadAsync(memory, cancellationToken);
                stream.Seek(0, SeekOrigin.Begin);
                await _bucket.UploadAsync(objectName, stream, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        private static string GetObjectName(
            string eventId,
            string fileId,
            FileType fileType)
        {
            var sb = new StringBuilder();

            sb.Append($"{eventId}/");
            sb.Append($"{fileId}.{fileType.ToString().ToLowerInvariant()}");

            return sb.ToString().TrimEnd('/');
        }

        public Task<Uri> ObtainGetPresignedUrl(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(fileId, nameof(fileId));

            var objectName = GetObjectName(eventId, fileId, fileType);

            try
            {
                var url = _bucket.GenerateGetPresignedUrl(objectName, cancellationToken);
                return url;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public Task<Uri> ObtainPutPresignedUrl(
            string eventId,
            string fileId,
            FileType fileType,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(fileId, nameof(fileId));

            var objectName = GetObjectName(eventId, fileId, fileType);

            try
            {
                var url = _bucket.GeneratePutPresignedUrl(objectName, cancellationToken);
                return url;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }
    }
}
