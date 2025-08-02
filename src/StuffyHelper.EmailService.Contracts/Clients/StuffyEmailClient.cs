using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Web;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Contracts.Clients;

/// <inheritdoc cref="StuffyHelper.EmailService.Contracts.Clients.Interfaces.IStuffyEmailClient" />
public class StuffyEmailClient : ApiClientBase, IStuffyEmailClient
{
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
        var request = CreateRequest(KnownRoutes.SendEmailRoute)
            .AddJsonBody(body)
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
}