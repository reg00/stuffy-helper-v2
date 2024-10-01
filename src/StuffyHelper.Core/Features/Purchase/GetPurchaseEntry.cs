using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.PurchaseTag;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.UnitType;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class GetPurchaseEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
        [Required]
        public double Cost { get; init; }
        [Required]
        public double Amount { get; init; }
        [Required]
        public bool IsPartial { get; init; }
        [Required]
        public bool IsComplete { get; init; }

        [Required]
        public EventShortEntry? Event { get; init; }
        public List<PurchaseTagShortEntry> PurchaseTags { get; init; }
        [Required]
        public UnitTypeShortEntry? UnitType { get; init; }
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; init; }
        public ParticipantShortEntry Participant { get; init; }

        public GetPurchaseEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            IsPartial = entry.IsPartial;
            IsComplete = entry.IsComplete;
            Event = new EventShortEntry(entry.Event);
            PurchaseUsages = entry.PurchaseUsages.Select(x => new PurchaseUsageShortEntry(x));
            PurchaseTags = entry.PurchaseTags.Select(x => new PurchaseTagShortEntry(x)).ToList();
            Participant = new ParticipantShortEntry(entry.Owner);
            UnitType = entry.UnitType == null ? null : new UnitTypeShortEntry(entry.UnitType);
        }
    }
}
