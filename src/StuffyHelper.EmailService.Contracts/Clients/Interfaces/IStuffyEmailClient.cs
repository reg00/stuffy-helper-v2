using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Contracts.Clients.Interfaces;

public interface IStuffyEmailClient
{
    public Task SendAsync(
        SendEmailRequest body,
        CancellationToken cancellationToken = default);
}