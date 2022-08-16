using EnsureThat;
using StuffyHelper.Core.Features.PurchaseType;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Features.Purchase
{
    public class GetPurchaseEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
        public GetShoppingEntry? Shopping { get; set; }
        public GetPurchaseTypeEntry? PurchaseType { get; set; }
        public List<GetPurchaseUsageEntry> PurchaseUsages { get; set; }

        public GetPurchaseEntry()
        {
            PurchaseUsages = new List<GetPurchaseUsageEntry>();
        }

        public GetPurchaseEntry(PurchaseEntry entry, bool includeShopping, bool includePurchaseUsages, bool includePurchaseType)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Amount = entry.Amount;
            Weight = entry.Weight;
            Count = entry.Count;
            IsActive = entry.IsActive;
            Shopping = includeShopping ? new GetShoppingEntry(entry.Shopping, false, false, false) : null;
            PurchaseUsages = includePurchaseUsages ? entry.PurchaseUsages.Select(x => new GetPurchaseUsageEntry(x, false, false)).ToList() : new List<GetPurchaseUsageEntry>();
            PurchaseType = includePurchaseType ? new GetPurchaseTypeEntry(entry.PurchaseType, false) : null;
        }
    }
}
