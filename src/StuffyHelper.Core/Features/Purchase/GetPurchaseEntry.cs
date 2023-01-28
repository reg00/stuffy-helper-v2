using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class GetPurchaseEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }

        [Required]
        public EventShortEntry? Event { get; set; }
        public List<PurchaseTagShortEntry> PurchaseTags { get; set; }
        [Required]
        public UnitTypeShortEntry? UnitType { get; set; }
        public List<PurchaseUsageShortEntry> PurchaseUsages { get; set; }

        public GetPurchaseEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            Event = new EventShortEntry(entry.Event);
            PurchaseUsages = entry.PurchaseUsages.Select(x => new PurchaseUsageShortEntry(x)).ToList();
            PurchaseTags = entry.PurchaseTags.Select(x => new PurchaseTagShortEntry(x)).ToList();
            UnitType = new UnitTypeShortEntry(entry.UnitType);
        }
    }
}
