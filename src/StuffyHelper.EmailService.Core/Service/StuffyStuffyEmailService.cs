using EnsureThat;
using NETCore.MailKit.Core;
using StuffyHelper.EmailService.Core.Models;
using StuffyHelper.EmailService.Core.Service.Interfaces;

namespace StuffyHelper.EmailService.Core.Service
{
    public class StuffyStuffyEmailService : IStuffyEmailService
    {
        private readonly IEmailService _emailService;

        public StuffyStuffyEmailService(IEmailService emailService)
        {
            EnsureArg.IsNotNull(emailService, nameof(emailService));

            _emailService = emailService;
        }

        public async Task SendEmailAsync(SendEmailRequest request)
        {
            await _emailService.SendAsync(request.Email, request.Subject, request.Message, true);
        }
    }
}
