using Microsoft.Extensions.Options;
using StuffyHelper.Authorization.Contracts.Clients;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Contracts.Clients;
using StuffyHelper.Contracts.Clients.Interface;

namespace StuffyHelper.ApiGateway.Registration
{
    public static class ServicesRegistrationExtensions
    {
        /// <summary>
        /// Add DI clients
        /// </summary>
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetConfig();
        
            if(!config.Endpoints.TryGetValue("AuthorizationEndpoint", out var authEndpoint))
                throw new Exception("Cannot find Authorization service endpoint");
            if(!config.Endpoints.TryGetValue("StuffyEndpoint", out var stuffyEndpoint))
                throw new Exception("Cannot find Stuffy helper service endpoint");

            services.AddScoped<IAuthorizationClient>(_ => new AuthorizationClient(authEndpoint));
            services.AddScoped<IFriendClient>(_ => new FriendClient(authEndpoint));
            services.AddScoped<IFriendRequestClient>(_ => new FriendRequestClient(authEndpoint));
            services.AddScoped<IDebtClient>(_ => new DebtClient(stuffyEndpoint));
            services.AddScoped<IEventClient>(_ => new EventClient(stuffyEndpoint));
            services.AddScoped<IMediaClient>(_ => new MediaClient(stuffyEndpoint));
            services.AddScoped<IParticipantClient>(_ => new ParticipantClient(stuffyEndpoint));
            services.AddScoped<IPurchaseClient>(_ => new PurchaseClient(stuffyEndpoint));
            services.AddScoped<IPurchaseTagClient>(_ => new PurchaseTagClient(stuffyEndpoint));
            services.AddScoped<IPurchaseUsageClient>(_ => new PurchaseUsageClient(stuffyEndpoint));
            services.AddScoped<IUnitTypeClient>(_ => new UnitTypeClient(stuffyEndpoint));

            return services;
        }
        
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetConfig();
            var frontEndOptions = config.Frontend;

            services.AddSingleton(Options.Create(frontEndOptions));

            // services.AddScoped<IEventService, EventService>();
            // services.AddScoped<IParticipantService, ParticipantService>();
            // services.AddScoped<IPurchaseService, PurchaseService>();
            // services.AddScoped<IPurchaseUsageService, PurchaseUsageService>();
            // services.AddScoped<IPurchaseTagService, PurchaseTagService>();
            // services.AddScoped<IUnitTypeService, UnitTypeService>();
            // services.AddScoped<IPurchaseTagPipeline, PurchaseTagPipeline>();
            // services.AddScoped<IMediaService, MediaService>();
            // services.AddScoped<IDebtService, DebtService>();

            return services;
        }
    }
}
