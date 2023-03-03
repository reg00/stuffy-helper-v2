using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Debt
{
    public interface IDebtStore
    {
        Task<DebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry?> GetDebtAsync(string borrowerId, string debtorId, Guid eventId, CancellationToken cancellationToken = default);

        Task<Response<DebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? borrowerId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        Task<IEnumerable<DebtEntry>> GetDebtsByUserAsync(
            string userId,
            CancellationToken cancellationToken = default);

        Task<DebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry> UpdateDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);
    }
}
