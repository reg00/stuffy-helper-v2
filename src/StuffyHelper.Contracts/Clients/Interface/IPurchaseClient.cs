using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IPurchaseClient
{
    public Task<Response<GetPurchaseEntry>> GetPurchasesAsync(
        string token,
        int offset = 0,
        int limit = 10,
        string? name = null,
        double? costMin = null,
        double? costMax = null,
        Guid? eventId = null,
        string[]? purchaseTags = null,
        Guid? unitTypeId = null,
        bool? isComplete = null,
        CancellationToken cancellationToken = default);

    public Task<GetPurchaseEntry> GetPurchaseAsync(
        string token,
        Guid purchaseId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseShortEntry> CreatePurchaseAsync(
        string token,
        AddPurchaseEntry body,
        CancellationToken cancellationToken = default);

    public Task DeletePurchaseAsync(
        string token,
        Guid purchaseId,
        CancellationToken cancellationToken = default);

    public Task<PurchaseShortEntry> UpdatePurchaseAsync(
        string token,
        Guid purchaseId,
        UpdatePurchaseEntry body,
        CancellationToken cancellationToken = default);
}