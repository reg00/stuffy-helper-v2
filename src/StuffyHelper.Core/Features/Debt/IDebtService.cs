using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Debt
{
    public interface IDebtService
    {
        Task<GetDebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken);

        Task<Response<GetDebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? borrowerId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<GetDebtEntry>> GetDebtsByUserAsync(
            string userId,
            CancellationToken cancellationToken = default);

        //Task<GetDebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        //Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<GetDebtEntry> SentDebtAsync(Guid debtId, double amount, CancellationToken cancellationToken = default);

        Task<GetDebtEntry> ConfirmDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task CheckoutEvent(Guid eventId, string userId, CancellationToken cancellationToken = default);
    }
}
