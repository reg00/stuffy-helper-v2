using EnsureThat;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Data;
using StuffyHelper.Authorization.Data.Repository;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;
using StuffyHelper.Common.Configurators;
using StuffyHelper.EmailService.Contracts.Clients;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;

namespace StuffyHelper.Authorization.Api.Registration;

public static class AuthorizationRegistrationExtensions
{
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetConfig();
        
        if(!config.Endpoints.TryGetValue("EmailEnpoint", out var baseUrl))
            throw new Exception("Cannot find Email enpdoint");

        services.AddScoped<IStuffyEmailClient>(_ => new StuffyEmailClient(baseUrl));

        return services;
    }
    
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetConfig().AuthorizationConfiguration;

        EnsureArg.IsNotNull(config, nameof(config));

        services.AddDbContext<UserDbContext>(
            options => options.UseNpgsql(config.ConnectionString));

        services.AddIdentity<StuffyUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<UserDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IInitializer, DatabaseInitializer>();
        services.AddScoped<IFriendRequestRepository, FriendRequestRepository>();
        services.AddScoped<IFriendRepository, FriendRepository>();
        services.AddScoped<IAvatarRepository, AvatarRepository>();

        return services;
    }

    public static IServiceProvider AddAuthDatabaseMigration(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        dbContext.Database.Migrate();

        return services;
    }
}