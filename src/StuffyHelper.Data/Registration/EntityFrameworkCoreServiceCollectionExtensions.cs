using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Data.Storage;

namespace StuffyHelper.Data.Registration
{
    /// <summary>
    /// EF service extensions
    /// </summary>
    public static class EntityFrameworkCoreServiceCollectionExtensions
    {
        /// <summary>
        /// Add db store if EF
        /// </summary>
        public static IServiceCollection AddDbStore<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<NpgsqlDbContextOptionsBuilder>? configurePostgreSql = null)
            where TDbContext : DbContext
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var entityFrameworkConfiguration = new EntityFrameworkConfiguration();
            configuration.GetSection(EntityFrameworkConfiguration.DefaultSectionName)
                .Bind(entityFrameworkConfiguration);

            services.AddSingleton(Options.Create(entityFrameworkConfiguration));
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                    .UseNpgsql(entityFrameworkConfiguration.ConnectionString, configurePostgreSql)
                    .EnableDetailedErrors()
                    .EnableSensitiveDataLogging();
            });

            return services;
        }
    }
}
