using StuffyHelper.EmailService.Core.Models;

namespace StuffyHelper.EmailService.Core.Service.Interfaces
{
    public interface IStuffyEmailService
    {
        Task SendEmailAsync(SendEmailRequest request);
    }
}
