using EnsureThat;
using StuffyHelper.Core.Features.PurchaseType;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public Guid ShoppingId { get; set; }
        public Guid PurchaseTypeId { get; set; }
        public Guid UnitTypeId { get; set; }
        public bool IsActive { get; set; }

        public virtual ShoppingEntry Shopping { get; set; }
        public virtual PurchaseTypeEntry PurchaseType { get; set; }
        public virtual UnitTypeEntry UnitType { get; set; }
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertPurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Amount = entry.Amount;
            Weight = entry.Weight;
            Count = entry.Count;
            ShoppingId = entry.ShoppingId;
            PurchaseTypeId = entry.PurchaseTypeId;
            IsActive = entry.IsActive;
        }
    }
}
