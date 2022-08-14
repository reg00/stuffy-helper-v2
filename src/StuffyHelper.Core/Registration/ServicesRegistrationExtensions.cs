using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Registration
{
    public static class ServicesRegistrationExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IEventService, EventService>();

            return services;
        }
    }
}
