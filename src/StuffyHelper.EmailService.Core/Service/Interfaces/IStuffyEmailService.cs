using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Core.Service.Interfaces
{
    public interface IStuffyEmailService
    {
        Task SendEmailAsync(SendEmailRequest request);
    }
}
