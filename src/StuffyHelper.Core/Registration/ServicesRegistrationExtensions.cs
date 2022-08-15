using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Registration
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IParticipantService, ParticipantService>();
            services.AddScoped<IPurchaseService, PurchaseService>();
            services.AddScoped<IPurchaseUsageService, PurchaseUsageService>();
            services.AddScoped<IShoppingService, ShoppingService>();

            return services;
        }
    }
}
