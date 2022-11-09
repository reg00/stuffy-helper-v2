namespace StuffyHelper.EmailService.Core.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string login, string message);
    }
}
