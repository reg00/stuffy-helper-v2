using EnsureThat;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using StuffyHelper.Core.Features.UnitType;
using System.Reflection.PortableExecutable;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseEntry : ITaggableEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Cost { get; set; }
        public double Amount { get; set; }
        public Guid ShoppingId { get; set; }
        public Guid UnitTypeId { get; set; }

        public virtual ShoppingEntry Shopping { get; set; }
        public virtual List<PurchaseTagEntry> PurchaseTags { get; set; } = new List<PurchaseTagEntry>();
        public virtual UnitTypeEntry UnitType { get; set; }
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public PurchaseEntry()
        {

        }

        public PurchaseEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Amount = entry.Amount;
            Cost = entry.Cost;
            Name = entry.Name;
            ShoppingId = entry.ShoppingId;
            UnitTypeId = entry.UnitTypeId;
            PurchaseTags = entry.PurchaseTags.Where(x => x.IsActive == true).ToList();
            PurchaseUsages = entry.PurchaseUsages;
            Shopping = entry.Shopping;
            UnitType = entry.UnitType;
        }

        public void PatchFrom(UpdatePurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            UnitTypeId = entry.UnitTypeId;
        }
    }
}
