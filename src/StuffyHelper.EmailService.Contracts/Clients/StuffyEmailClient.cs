using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.EmailService.Contracts.Clients.Interfaces;
using StuffyHelper.EmailService.Contracts.Models;

namespace StuffyHelper.EmailService.Contracts.Clients;

public class StuffyEmailClient : ApiClientBase, IStuffyEmailClient
{
    public StuffyEmailClient(string baseUrl) : base(baseUrl)
    {
    }
    
    
    public Task SendAsync(
        SendEmailRequest body,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest("Email/send")
            .AddJsonBody(body);

        return Post(request, cancellationToken);
    }
}