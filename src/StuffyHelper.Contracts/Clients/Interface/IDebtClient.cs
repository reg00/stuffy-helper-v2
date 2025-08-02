using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IDebtClient
{
    public Task<Response<GetDebtEntry>> GetDebtsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        CancellationToken cancellationToken = default);

    public Task<GetDebtEntry> GetDebtAsync(
        string token,
        Guid debtId,
        CancellationToken cancellationToken = default);
    
    public Task SendDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);

    public Task ConfirmDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);
}