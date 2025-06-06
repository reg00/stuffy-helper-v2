using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Data.Repository;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Schema;

namespace StuffyHelper.Data.Registration
{
    public static class DbServicesRegistrationExtensions
    {
        public static IServiceCollection AddEfDbServices(this IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IParticipantRepository, ParticipantRepository>();
            services.AddScoped<IPurchaseRepository, PurchaseRepository>();
            services.AddScoped<IPurchaseUsageRepository, PurchaseUsageRepository>();
            services.AddScoped<IPurchaseTagRepository, EfPurchaseTagRepository>();
            services.AddScoped<IUnitTypeRepository, UnitTypeRepository>();
            services.AddScoped<IMediaRepository, MediaRepository>();
            services.AddScoped<IDebtRepository, DebtRepository>();
            services.AddScoped<ICheckoutRepository, CheckoutRepository>();

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
