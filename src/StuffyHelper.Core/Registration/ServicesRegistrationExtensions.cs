using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StuffyHelper.Authorization.Contracts.Clients;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Common.Configurators;
using StuffyHelper.Core.Configs;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseTag.Pipeline;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.Core.Registration
{
    public static class ServicesRegistrationExtensions
    {
        /// <summary>
        /// Add DI clients
        /// </summary>
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {
            var config = configuration.GetConfig();
        
            if(!config.Endpoints.TryGetValue("AuthorizationEnpoint", out var baseUrl))
                throw new Exception("Cannot find Email enpdoint");

            services.AddScoped<IAuthorizationClient>(_ => new AuthorizationClient(baseUrl));
            services.AddScoped<IFriendClient>(_ => new FriendClient(baseUrl));
            services.AddScoped<IFriendRequestClient>(_ => new FriendRequestClient(baseUrl));

            return services;
        }
        
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            var frontEndOptions = new FrontEndConfiguration();
            configuration.GetSection(FrontEndConfiguration.DefaultSectionName)
                .Bind(frontEndOptions);

            services.AddSingleton(Options.Create(frontEndOptions));

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
    }
}
