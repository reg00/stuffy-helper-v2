using RestSharp;
using StuffyHelper.Common.Client;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients;

public class DebtClient: ApiClientBase, IDebtClient
{
    public DebtClient(string baseUrl) : base(baseUrl)
    {
        
    }
    
    public Task<Response<GetDebtEntry>> GetDebtsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetDebtsRoute)
            .AddBearerToken(token)
            .AddQueryParameter("limit", limit)
            .AddQueryParameter("offset", offset);

        return Get<Response<GetDebtEntry>>(request, cancellationToken);
    }

    public Task<GetDebtEntry> GetDebtAsync(
        string token,
        Guid debtId,
        CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.GetDebtRoute)
            .AddBearerToken(token);

        return Get<GetDebtEntry>(request, cancellationToken);
    }

    public Task SendDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.SendDebtRoute)
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
    
    public Task ConfirmDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default)
    {
        var request = CreateRequest(KnownRoutes.ConfirmDebtRoute)
            .AddBearerToken(token);

        return Post(request, cancellationToken);
    }
}