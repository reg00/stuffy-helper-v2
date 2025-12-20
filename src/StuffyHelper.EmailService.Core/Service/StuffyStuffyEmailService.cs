using EnsureThat;
using NETCore.MailKit.Core;
using StuffyHelper.EmailService.Contracts.Models;
using StuffyHelper.EmailService.Core.Service.Interfaces;

namespace StuffyHelper.EmailService.Core.Service
{
    /// <inheritdoc />
    public class StuffyStuffyEmailService : IStuffyEmailService
    {
        private readonly IEmailService _emailService;

        /// <summary>
        /// Ctor.
        /// </summary>
        public StuffyStuffyEmailService(IEmailService emailService)
        {
            EnsureArg.IsNotNull(emailService, nameof(emailService));

            _emailService = emailService;
        }

        /// <inheritdoc />
        public async Task SendEmailAsync(SendEmailRequest request)
        {
            await _emailService.SendAsync(request.Email, request.Subject, request.Message, true);
        }
    }
}
