using EnsureThat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Reg00.Infrastructure.Minio.Configs;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Minio.Configs;
using StuffyHelper.Minio.Storage;

namespace StuffyHelper.Minio.Registration
{
    public static class ServiceCollectionMinioExtensions
    {
        public static IServiceCollection AddMinioBlobDataStores(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var minioClientOptions = new MinioClientOptions();
            configuration.GetSection(MinioClientOptions.DefaultSectionName)
                .Bind(minioClientOptions);

            services
                .AddMinioClient(minioClientOptions)
                .AddMinioBlobStore(minioClientOptions);

            return services;
        }

        private static IServiceCollection AddMinioClient(this IServiceCollection services, MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(minioClientOptions, nameof(minioClientOptions));

            var minioClient = new MinioClient()
                .WithEndpoint(minioClientOptions.Endpoint)
                .WithCredentials(minioClientOptions.AccessKey, minioClientOptions.SecretKey)
                .WithTimeout(minioClientOptions.NetworkTimeout);

            services.AddSingleton(sp => minioClient);

            return services;
        }

        private static IServiceCollection AddMinioBlobStore(this IServiceCollection services, MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(minioClientOptions, nameof(minioClientOptions));

            var options = new MinioBlobStoreConfigurationSection();

            services.AddScoped<IFileStore>(sp =>
            {
                var minioClient = sp.GetRequiredService<MinioClient>();
                return new MinioFileStore(minioClient, options.BucketConfigurationName, minioClientOptions);
            });

            return services;
        }
    }
}
