using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.EntityFrameworkCore.Features.Storage;

namespace StuffyHelper.EntityFrameworkCore.Registration
{
    public static class DbServicesRegistrationExtensions
    {
        public static IServiceCollection AddEfDbServices(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, EfEventStore>();
            services.AddScoped<IParticipantStore, EfParticipantStore>();
            services.AddScoped<IPurchaseStore, EfPurchaseStore>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }
    }
}
