using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
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
    }
}
