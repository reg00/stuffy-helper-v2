using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using StuffyHelper.EntityFrameworkCore.Configs;

namespace StuffyHelper.EntityFrameworkCore.Registration
{
    public static class EntityFrameworkCoreServiceCollectionExtensions
    {
        public static IServiceCollection AddDbStore<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<NpgsqlDbContextOptionsBuilder> configurePostgreSql = null)
            where TDbContext : DbContext
        {
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var entityFrameworkConfiguration = new EntityFrameworkConfiguration();
            configuration.GetSection(Constants.EntityFrameworkConfigurationSection)
                .Bind(entityFrameworkConfiguration);

            services.AddSingleton(Options.Create(entityFrameworkConfiguration));
            services.AddDbContext<TDbContext>(options =>
            {
                options.UseNpgsql(entityFrameworkConfiguration.ConnectionString, configurePostgreSql);
            });

            return services;

        }
    }
}
