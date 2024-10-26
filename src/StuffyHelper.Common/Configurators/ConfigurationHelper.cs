using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StuffyHelper.Common.Configurations;

namespace StuffyHelper.Common.Configurators;

public static class ConfigurationHelper
{
    public static StuffyConfiguration GetConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection(AuthorizationConfiguration.DefaultSectionName)
            .Get<StuffyConfiguration>();

        if (config == null)
            throw new Exception("Cannot find Configuration");

        return config;
    }
    
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, string environment)
    {
        var a = AppContext.BaseDirectory;
        var b = Directory.GetCurrentDirectory();
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);

        var configuration = builder.Build();
        
        // Регистрация MySettings как конфигурационной секции
        var stuffyConfig = new StuffyConfiguration();
        configuration.GetSection(StuffyConfiguration.DefaultSection)
            .Bind(stuffyConfig);

        services.AddSingleton(Options.Create(stuffyConfig));
        
        return services;
    }
}