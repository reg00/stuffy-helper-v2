using EnsureThat;
using Microsoft.Extensions.Options;
using Minio;
using Reg00.Infrastructure.Minio.Configs;
using Reg00.Infrastructure.Minio.Extensions;
using Reg00.Infrastructure.Minio.Features.Client;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Minio.Configs;
using StuffyHelper.Minio.Features.Exceptions;

namespace StuffyHelper.Minio.Features.Storage
{
    public class MinioFileStore : IFileStore
    {
        private readonly MinioBucketClient _bucket;
        private readonly MinioClient _client;

        public MinioFileStore(MinioClient client,
            IOptionsMonitor<BlobContainerConfiguration> namedBlobBucketConfigurationAccessor,
            MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(client, nameof(client));
            EnsureArg.IsNotNull(namedBlobBucketConfigurationAccessor, nameof(namedBlobBucketConfigurationAccessor));

            var bucketConfiguration = namedBlobBucketConfigurationAccessor
                .Get(Constants.BlobBucketConfigurationName);

            _client = client;
            _bucket = client.GetBucketClientAsync(minioClientOptions, bucketConfiguration.ContainerName).Result;
            SetPublicPolicy(bucketConfiguration.ContainerName);
        }

        public MinioFileStore(MinioClient client,
            string bucketName,
            MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(client, nameof(client));
            EnsureArg.IsNotNull(bucketName, nameof(bucketName));

            _bucket = client.GetBucketClientAsync(minioClientOptions, bucketName).Result;
            _client = client;
            SetPublicPolicy(bucketName);
        }

        private void SetPublicPolicy(string bucketName)
        {
            string policyJson = $@"{{""Version"": ""2012-10-17"",""Statement"": [{{""Action"": [""s3:GetObject""], ""Effect"": ""Allow"", ""Principal"": {{ ""AWS"": [ ""*"" ] }}, ""Resource"": [ ""arn:aws:s3:::{bucketName}/*"" ] }} ] }}";
            var policyArgs = new SetPolicyArgs()
                .WithBucket(bucketName)
                .WithPolicy(policyJson);
            _client.SetPolicyAsync(policyArgs).ConfigureAwait(false);
        }

        public async Task DeleteFilesIfExistAsync(
            string objectName,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(objectName, nameof(objectName));

            try
            {
                await _bucket.DeleteIfExistsAsync(objectName, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new StuffyMinioException(ex);
            }
        }

        public async Task<Stream> GetFileAsync(
            string objectName,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(objectName, nameof(objectName));

            try
            {
                return await _bucket.DownloadAsync(objectName, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new StuffyMinioException(ex);
            }
        }

        public async Task StoreFileAsync(
            string objectName,
            Stream stream,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotEmptyOrWhiteSpace(objectName, nameof(objectName));
            EnsureArg.IsNotNull(stream, nameof(stream));

            try
            {
                var memory = new Memory<byte>();
                await stream.ReadAsync(memory, cancellationToken);
                stream.Seek(0, SeekOrigin.Begin);
                await _bucket.UploadAsync(objectName, stream, cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                throw new StuffyMinioException(ex);
            }
        }

        public async Task<Uri> ObtainGetPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(objectName, nameof(objectName));

            try
            {
                var url = await _bucket.GenerateGetPresignedUrl(objectName, cancellationToken);
                return new Uri(url.GetLeftPart(UriPartial.Path));
            }
            catch (Exception ex)
            {
                throw new StuffyMinioException(ex);
            }
        }

        public Task<Uri> ObtainPutPresignedUrl(
            string objectName,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(objectName, nameof(objectName));

            try
            {
                var url = _bucket.GeneratePutPresignedUrl(objectName, cancellationToken);
                return url;
            }
            catch (Exception ex)
            {
                throw new StuffyMinioException(ex);
            }
        }
    }
}
