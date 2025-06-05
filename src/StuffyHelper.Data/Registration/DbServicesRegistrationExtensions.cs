using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;
using StuffyHelper.Data.Features.Schema;
using StuffyHelper.Data.Features.Storage;

namespace StuffyHelper.Data.Registration
{
    public static class DbServicesRegistrationExtensions
    {
        public static IServiceCollection AddEfDbServices(this IServiceCollection services)
        {
            services.AddScoped<IEventStore, EfEventStore>();
            services.AddScoped<IParticipantStore, EfParticipantStore>();
            services.AddScoped<IPurchaseStore, EfPurchaseStore>();
            services.AddScoped<IPurchaseUsageStore, EfPurchaseUsageStore>();
            services.AddScoped<IPurchaseTagStore, EfPurchaseTagStore>();
            services.AddScoped<IUnitTypeStore, EfUnitTypeStore>();
            services.AddScoped<IMediaStore, EfMediaStore>();
            services.AddScoped<IDebtStore, EfDebtStore>();
            services.AddScoped<ICheckoutStore, EfCheckoutStore>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }

        public static IServiceProvider AddEfDatabaseMigration(this IServiceProvider services)
        {
            using (var scope = services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<StuffyHelperContext>();

                dbContext.Database.Migrate();
            }

            return services;
        }
    }
}
