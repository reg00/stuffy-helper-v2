using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class PurchaseShortEntry
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

        public List<PurchaseTagShortEntry> PurchaseTags { get; init; }
        public UnitTypeShortEntry? UnitType { get; init; }

        public PurchaseShortEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Amount = entry.Amount;
            IsPartial = entry.IsPartial;
            IsComplete = entry.IsComplete;
            UnitType = entry.UnitType == null ? null : new UnitTypeShortEntry(entry.UnitType);
            PurchaseTags = entry.PurchaseTags.Select(x => new PurchaseTagShortEntry(x)).ToList();
        }
    }
}
