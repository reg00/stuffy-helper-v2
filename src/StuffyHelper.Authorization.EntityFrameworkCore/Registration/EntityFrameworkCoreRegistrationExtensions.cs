using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Authorization.Core.Configs;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Features.FriendsRequest;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Authorization.EntityFrameworkCore.Features;
using StuffyHelper.Authorization.EntityFrameworkCore.Features.Schema;
using StuffyHelper.Authorization.EntityFrameworkCore.Features.Storage;

namespace StuffyHelper.Authorization.EntityFrameworkCore.Registration
{
    public static class EntityFrameworkCoreRegistrationExtensions
    {
        public static IServiceCollection AddEfAuthDbServices(this IServiceCollection services, IConfiguration configuration)
        {
            var authOptions = configuration.GetSection(AuthorizationConfiguration.DefaultSectionName)
                                                    .Get<AuthorizationConfiguration>();

            EnsureArg.IsNotNull(authOptions, nameof(authOptions));

            services.AddDbContext<UserDbContext>(
                options => options.UseNpgsql(authOptions.ConnectionString));

            services.AddIdentity<StuffyUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
            })
                    .AddEntityFrameworkStores<UserDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<IInitializer, EfInitializer>();
            services.AddScoped<IFriendsRequestStore, EfFriendsRequestStorage>();

            return services;
        }

        public static IServiceProvider AddAuthDatabaseMigration(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}
