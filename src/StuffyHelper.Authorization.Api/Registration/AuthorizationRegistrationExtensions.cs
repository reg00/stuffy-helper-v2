using System.Text;
using EnsureThat;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Core.Services;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data;
using StuffyHelper.Authorization.Data.Repository;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Authorization.Data.Storage;
using StuffyHelper.Common.Configurations;
using StuffyHelper.Common.Configurators;
using StuffyHelper.EmailService.Contracts.Clients;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.Minio.Registration;
using IAuthorizationService = StuffyHelper.Authorization.Core.Services.Interfaces.IAuthorizationService;

namespace StuffyHelper.Authorization.Api.Registration;

/// <summary>
/// Authorization registration extensions
/// </summary>
public static class AuthorizationRegistrationExtensions
{
    /// <summary>
    /// Add DI clients
    /// </summary>
    public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetConfig();
        
        if(!config.Endpoints.TryGetValue("EmailEnpoint", out var baseUrl))
            throw new Exception("Cannot find Email enpdoint");

        services.AddScoped<IStuffyEmailClient>(_ => new StuffyEmailClient(baseUrl));

        return services;
    }
    
    /// <summary>
    /// Add services and database
    /// </summary>
    public static IServiceCollection AddAuthServices(this IServiceCollection services, IConfiguration configuration)
    {
        var config = configuration.GetConfig().Authorization;

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
        
        services.AddAuthentificationServices();
        services.AddMinioBlobDataStores(configuration.GetSection(StuffyConfiguration.DefaultSection));

        return services;
    }
    
    /// <summary>
    /// Add services
    /// </summary>
    private static IServiceCollection AddAuthentificationServices(this IServiceCollection services)
    {
        services.AddScoped<IInitializer, DatabaseInitializer>();
        services.AddTransient<IFriendRequestRepository, FriendRequestRepository>();
        services.AddTransient<IFriendRepository, FriendRepository>();
        services.AddTransient<IAvatarRepository, AvatarRepository>();
        
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IFriendsRequestService, FriendsRequestService>();
        services.AddScoped<IFriendService, FriendService>();
        services.AddScoped<IAvatarService, AvatarService>();

        return services;
    }

    /// <summary>
    /// Add auth database migration
    /// </summary>
    public static IServiceProvider AddAuthDatabaseMigration(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<UserDbContext>();

        dbContext.Database.Migrate();

        return services;
    }
}