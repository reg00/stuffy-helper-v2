using EnsureThat;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.Core.Features.UnitType;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class GetPurchaseEntry
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public GetShoppingEntry? Shopping { get; set; }
        public List<GetPurchaseTagEntry> PurchaseTags { get; set; }
        [Required]
        public GetUnitTypeEntry? UnitType { get; set; }
        public List<GetPurchaseUsageEntry> PurchaseUsages { get; set; }

        public GetPurchaseEntry()
        {
            PurchaseUsages = new List<GetPurchaseUsageEntry>();
        }

        public GetPurchaseEntry(PurchaseEntry entry, bool includeShopping, bool includePurchaseUsages, bool includeUnitType)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Weight = entry.Weight;
            Count = entry.Count;
            IsActive = entry.IsActive;
            Shopping = includeShopping ? new GetShoppingEntry(entry.Shopping, false, false, false) : null;
            PurchaseUsages = includePurchaseUsages ? entry.PurchaseUsages.Select(x => new GetPurchaseUsageEntry(x, false, false)).ToList() : new List<GetPurchaseUsageEntry>();
            PurchaseTags = entry.PurchaseTags.Select(x => new GetPurchaseTagEntry(x, false)).ToList();
            UnitType = includeUnitType ? new GetUnitTypeEntry(entry.UnitType, false) : null;
        }
    }
}
