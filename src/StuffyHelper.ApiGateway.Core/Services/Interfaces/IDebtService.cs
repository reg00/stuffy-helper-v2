using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IDebtService
    {
        Task<GetDebtEntry> GetDebtAsync(string token, Guid debtId, CancellationToken cancellationToken);

        Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string token,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        Task SendDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);

        Task ConfirmDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);
    }
}
