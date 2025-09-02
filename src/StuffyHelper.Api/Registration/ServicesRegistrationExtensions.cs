using Microsoft.Extensions.Options;
using StuffyHelper.Authorization.Contracts.AutoMapper;
using StuffyHelper.Authorization.Contracts.Clients;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Contracts.AutoMapper;
using StuffyHelper.Core.Services;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Registration
{
    /// <summary>
    /// Service DI registration extensions
    /// </summary>
    public static class ServicesRegistrationExtensions
    {
        /// <summary>
        /// Add DI clients
        /// </summary>
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetConfig();
        
            if(!config.Endpoints.TryGetValue("AuthorizationEndpoint", out var baseUrl))
                throw new Exception("Cannot find Email endpoint");

            services.AddScoped<IAuthorizationClient>(_ => new AuthorizationClient(baseUrl));
            services.AddScoped<IFriendClient>(_ => new FriendClient(baseUrl));
            services.AddScoped<IFriendRequestClient>(_ => new FriendRequestClient(baseUrl));

            return services;
        }
        
        /// <summary>
        /// Add services
        /// </summary>
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetConfig();
            var frontEndOptions = config.Frontend;

            services.AddSingleton(Options.Create(frontEndOptions));

            services.AddMapping();
            
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPurchaseUsageService, PurchaseUsageService>();
            services.AddScoped<IPurchaseTagService, PurchaseTagService>();
            services.AddScoped<IUnitTypeService, UnitTypeService>();
            services.AddScoped<IPurchaseTagPipeline, PurchaseTagPipeline>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IDebtService, DebtService>();

            return services;
        }
        
        /// <summary>
        /// Add automapper mappings
        /// </summary>
        private static IServiceCollection AddMapping(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile(new FriendsRequestAutoMapperProfile());
                cfg.AddProfile(new FriendAutoMapperProfile());
                cfg.AddProfile(new UserAutoMapperProfile());
                cfg.AddProfile(new AvatarAutoMapperProfile());
                cfg.AddProfile(new MediaAutoMapperProfile());
                
                cfg.AddProfile(new DebtAutoMapperProfile());
                cfg.AddProfile(new CheckoutAutoMapperProfile());
                cfg.AddProfile(new EventAutoMapperProfile());
                cfg.AddProfile(new MediaAutoMapperProfile());
                cfg.AddProfile(new ParticipantAutoMapperProfile());
                cfg.AddProfile(new PurchaseAutoMapperProfile());
                cfg.AddProfile(new PurchaseTagAutoMapperProfile());
                cfg.AddProfile(new PurchaseUsageAutoMapperProfile());
                cfg.AddProfile(new UnitTypeAutoMapperProfile());
            });

            return services;
        }
    }
}
