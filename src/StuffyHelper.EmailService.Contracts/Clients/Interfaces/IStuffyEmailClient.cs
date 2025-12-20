using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Contracts.Clients.Interfaces;

/// <summary>
/// Stuffy email client
/// </summary>
public interface IStuffyEmailClient
{
    /// <summary>
    /// Send email message
    /// </summary>
    /// <param name="token">Auth token</param>
    /// <param name="body">Body</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public Task SendAsync(
        string token,
        SendEmailRequest body,
        CancellationToken cancellationToken = default);
}