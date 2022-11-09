using EnsureThat;

namespace StuffyHelper.EmailService.Core.Service
{
    public class EmailService : IEmailService
    {
        private readonly NETCore.MailKit.Core.IEmailService _emailService;

        public EmailService(NETCore.MailKit.Core.IEmailService emailService)
        {
            EnsureArg.IsNotNull(emailService, nameof(emailService));

            _emailService = emailService;
        }

        public async Task SendEmailAsync(string email, string login, string message)
        {
            await _emailService.SendAsync(email, login, message);
        }
    }
}
