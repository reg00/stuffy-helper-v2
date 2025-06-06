using StuffyHelper.Common.Messages;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Debt
{
    public interface IDebtService
    {
        Task<GetDebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken);

        Task<Response<GetDebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        //Task<GetDebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        //Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<GetDebtEntry> SendDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default);

        Task<GetDebtEntry> ConfirmDebtAsync(string userId, Guid debtId, CancellationToken cancellationToken = default);

        Task CheckoutEvent(Guid eventId, string? userId, CancellationToken cancellationToken = default);
    }
}
