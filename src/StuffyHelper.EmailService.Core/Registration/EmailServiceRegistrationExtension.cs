using EnsureThat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NETCore.MailKit.Extensions;
using NETCore.MailKit.Infrastructure.Internal;
using StuffyHelper.EmailService.Core.Configs;
using StuffyHelper.EmailService.Core.Service.Interfaces;

namespace StuffyHelper.EmailService.Core.Registration
{
    public static class EmailServiceRegistrationExtension
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            EnsureArg.IsNotNull(services, nameof(services));
            EnsureArg.IsNotNull(configuration, nameof(configuration));

            var emailServiceConfiguration = new EmailServiceConfiguration();
            configuration.GetSection(EmailServiceConfiguration.DefaultSectionName)
                .Bind(emailServiceConfiguration);

            services.AddSingleton(Options.Create(emailServiceConfiguration));

            services.AddMailKit(options =>
            {
                options.UseMailKit(new MailKitOptions()
                {
                    Server = emailServiceConfiguration.Server,
                    Port = emailServiceConfiguration.Port,
                    Account = emailServiceConfiguration.Account,
                    Password = emailServiceConfiguration.Password,
                    SenderEmail = emailServiceConfiguration.SenderEmail,
                    SenderName = emailServiceConfiguration.SenderName,
                    // Set it to TRUE to enable ssl or tls, FALSE otherwise
                    Security = true
                });
            });

            services.AddScoped<IStuffyEmailService, Service.StuffyStuffyEmailService>();

            return services;
        }
    }
}
