using Microsoft.Extensions.DependencyInjection;

namespace StuffyHelper.EntityFrameworkCore.Registration
{
    public static class DbServicesRegistrationExtensions
    {
        public static IServiceCollection AddEfDbServices(this IServiceCollection services)
        {
            //services.AddScoped<IIcd10Store, EfIcd10Store>();
            //services.AddScoped<IIcdOStore, EfIcdOStore>();
            //services.AddScoped<IMedicalInstitutionStore, EfMedicalInstitutionStore>();
            //services.AddScoped<IScannerStore, EfScannerStore>();
            //services.AddScoped<IScannerResolutionStore, EfScannerResolutionStore>();
            //services.AddScoped<IEmployeePositionStore, EfEmployeePositionStore>();
            //services.AddScoped<IEmployeeStore, EfEmployeeStore>();
            //services.AddScoped<IBiomaterialSourceStore, EfBiomaterialSourceStore>();
            //services.AddScoped<IDbInitializer, EfDbInitializer>();
            //services.AddScoped<IAuditStore, EfAuditStore>();
            //services.AddScoped<IOperationTypeStore, EfOperationTypeStore>();

            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            return services;
        }
    }
}
