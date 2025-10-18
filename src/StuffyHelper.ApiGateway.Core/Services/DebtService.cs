using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class DebtService : IDebtService
    {
        private readonly IDebtClient _debtClient;

        public DebtService(
            IDebtClient debtClient)
        {
            _debtClient = debtClient;
        }

        public async Task<GetDebtEntry> GetDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken)
        {
            return await _debtClient.GetDebtAsync(token, eventId, debtId, cancellationToken);
        }

        public async Task<Response<GetDebtEntry>> GetDebtsByUserAsync(
            string token,
            Guid eventId,
            int offset = 0,
            int limit = 10,
            CancellationToken cancellationToken = default)
        {
            return await _debtClient.GetDebtsAsync(token, eventId, offset, limit, cancellationToken);
        }

        public async Task SendDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
        {
            await _debtClient.SendDebtAsync(token, eventId, debtId, cancellationToken);
        }

        public async Task ConfirmDebtAsync(string token, Guid eventId, Guid debtId, CancellationToken cancellationToken = default)
        {
            await _debtClient.ConfirmDebtAsync(token, eventId, debtId, cancellationToken);
        }
    }
}
