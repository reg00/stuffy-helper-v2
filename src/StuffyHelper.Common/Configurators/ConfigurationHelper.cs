using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StuffyHelper.Common.Configurations;

namespace StuffyHelper.Common.Configurators;

/// <summary>
/// Helper for set up custom configuration
/// </summary>
public static class ConfigurationHelper
{
    /// <summary>
    /// Get configuration
    /// </summary>
    public static StuffyConfiguration GetConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection(StuffyConfiguration.DefaultSection)
            .Get<StuffyConfiguration>();

        if (config == null)
            throw new Exception("Cannot find Configuration");

        return config;
    }
    
    /// <summary>
    /// Add custom configuration file to pipeline
    /// </summary>
    public static IServiceCollection AddCustomConfiguration(this IServiceCollection services, ConfigurationManager configuration, string environment)
    {
        var assemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
    
        var builder = new ConfigurationBuilder()
            .SetBasePath(assemblyLocation)
            .AddJsonFile(Path.Combine(assemblyLocation, "appsettings.json"), optional: false, reloadOnChange: true)
            .AddJsonFile(Path.Combine(assemblyLocation, $"appsettings.{environment}.json"), optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();

        configuration.AddConfiguration(builder);
        
        // Регистрация MySettings как конфигурационной секции
        var stuffyConfig = new StuffyConfiguration();
        configuration.GetSection(StuffyConfiguration.DefaultSection)
            .Bind(stuffyConfig);

        services.AddSingleton(Options.Create(stuffyConfig));
        
        return services;
    }
}