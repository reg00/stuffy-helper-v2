using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.EmailService.Contracts.Clients.Interfaces.IStuffyEmailClient" />
public class StuffyEmailClient : ApiClientBase, IStuffyEmailClient
{
    private const string DefaultRoute = "api/v1/email";
    
    /// <inheritdoc />
    public StuffyEmailClient(string baseUrl) : base(baseUrl)
    {
    }
    
    /// <inheritdoc />
    public Task SendAsync(
        string token,
        SendEmailRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest($"{DefaultRoute}/send")
            .AddJsonBody(body)
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
}