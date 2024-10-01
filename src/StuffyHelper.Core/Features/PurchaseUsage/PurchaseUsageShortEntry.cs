using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageShortEntry
    {
        [Required]
        public Guid PurchaseUsageId { get; init; }
        public Guid PurchaseId { get; init; }
        public Guid ParticipantId { get; init; }
        public double Amount { get; init; }

        public PurchaseUsageShortEntry(PurchaseUsageEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            PurchaseUsageId = entry.Id;
            ParticipantId = entry.ParticipantId;
            PurchaseId = entry.PurchaseId;
            Amount = entry.Amount;
        }
    }
}
