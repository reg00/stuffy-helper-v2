using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Debt
{
    public interface IDebtStore
    {
        Task<DebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry?> GetDebtAsync(string lenderId, string debtorId, Guid eventId, CancellationToken cancellationToken = default);

        Task<PagedData<DebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        Task<PagedData<DebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        Task<DebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry> UpdateDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);
    }
}
