using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with debts
/// </summary>
public interface IDebtClient
{
    /// <summary>
    /// Return list of debts
    /// </summary>
    public Task<Response<GetDebtEntry>> GetDebtsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Return debt by id
    /// </summary>
    public Task<GetDebtEntry> GetDebtAsync(
        string token,
        Guid debtId,
        CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Pay debt
    /// </summary>
    public Task SendDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Confirm payment for debt
    /// </summary>
    public Task ConfirmDebtAsync(string token, Guid debtId, CancellationToken cancellationToken = default);
}