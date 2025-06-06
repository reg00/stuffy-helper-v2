using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface IDebtRepository
    {
        Task<DebtEntry> GetDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry?> GetDebtAsync(string lenderId, string debtorId, Guid eventId, CancellationToken cancellationToken = default);

        Task<Response<DebtEntry>> GetDebtsAsync(
            int offset = 0,
            int limit = 10,
            string? lenderId = null,
            string? debtorId = null,
            bool? isSent = null,
            bool? isConfirmed = null,
            CancellationToken cancellationToken = default);

        Task<Response<DebtEntry>> GetDebtsByUserAsync(
            string userId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default);

        Task<DebtEntry> AddDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);

        Task DeleteDebtAsync(Guid debtId, CancellationToken cancellationToken = default);

        Task<DebtEntry> UpdateDebtAsync(DebtEntry debt, CancellationToken cancellationToken = default);
    }
}
