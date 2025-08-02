using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Core.Service.Interfaces
{
    /// <summary>
    /// Stuffy email service
    /// </summary>
    public interface IStuffyEmailService
    {
        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="request">body</param>
        Task SendEmailAsync(SendEmailRequest request);
    }
}
