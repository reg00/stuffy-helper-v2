﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.EntityFrameworkCore.Features.Schema;
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
            services.AddScoped<IPurchaseUsageStore, EfPurchaseUsageStore>();
            services.AddScoped<IShoppingStore, EfShoppingStore>();

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