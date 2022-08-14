using EnsureThat;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
        public Guid ShoppingId { get; set; }

        public virtual ShoppingEntry Shopping { get; set; }
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertPurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Amount = entry.Amount;
            Count = entry.Count;
            ShoppingId = entry.ShoppingId;
        }
    }
}
