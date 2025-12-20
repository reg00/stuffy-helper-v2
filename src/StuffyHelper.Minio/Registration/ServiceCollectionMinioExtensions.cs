using EnsureThat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Minio;
using Reg00.Infrastructure.Minio.Configs;
using StuffyHelper.Minio.Configs;
using StuffyHelper.Minio.Features.Storage;

namespace StuffyHelper.Minio.Registration
{
    /// <summary>
    /// File store registration extensions
    /// </summary>
    public static class ServiceCollectionMinioExtensions
    {
        /// <summary>
        /// Add minio support
        /// </summary>
        public static IServiceCollection AddMinioBlobDataStores(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var minioClientOptions = new MinioClientOptions();
            configuration.GetSection(MinioClientOptions.DefaultSectionName)
                .Bind(minioClientOptions);

            services
                .AddMinioClient(minioClientOptions)
                .AddMinioBlobStore(configuration, minioClientOptions);

            return services;
        }

        /// <summary>
        /// Add minio client
        /// </summary>
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

        /// <summary>
        /// Add minio data store
        /// </summary>
        private static IServiceCollection AddMinioBlobStore(this IServiceCollection services, IConfiguration configuration, MinioClientOptions minioClientOptions)
        {
            EnsureArg.IsNotNull(minioClientOptions, nameof(minioClientOptions));

            var a = configuration.GetSection(FileStoreConfiguration.DefaultSection);
            
            var fileStoreConfiguration = new FileStoreConfiguration();
            configuration.GetSection(FileStoreConfiguration.DefaultSection)
               .Bind(fileStoreConfiguration);

            services.AddScoped<IFileStore>(sp =>
            {
                var minioClient = sp.GetRequiredService<MinioClient>();
                return new MinioFileStore(minioClient, fileStoreConfiguration.ContainerName, minioClientOptions);
            });

            return services;
        }
    }
}
