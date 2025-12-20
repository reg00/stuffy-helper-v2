using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with purchases
/// </summary>
public interface IPurchaseClient
{
    /// <summary>
    /// Return list of purchases from event
    /// </summary>
    public Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
        string token,
        Guid eventId,
        int offset = 0,
        int limit = 10,
        string? name = null,
        long? costMin = null,
        long? costMax = null,
        bool? isComplete = null,
        Guid[]? purchaseIds = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Return purchase by id
    /// </summary>
    public Task<GetPurchaseEntry> GetPurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Create new purchase into event
    /// </summary>
    public Task<PurchaseShortEntry> CreatePurchaseAsync(
        string token,
        Guid eventId,
        AddPurchaseEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove purchase from event
    /// </summary>
    public Task DeletePurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Update event purchase by id
    /// </summary>
    public Task<PurchaseShortEntry> UpdatePurchaseAsync(
        string token,
        Guid eventId,
        Guid purchaseId,
        UpdatePurchaseEntry body,
        CancellationToken cancellationToken = default);
}