using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageShortEntry
    {
        [Required]
        public Guid PurchaseUsageId { get; set; }
        public Guid PurchaseId { get; set; }
        public Guid ParticipantId { get; set; }
        public int Amount { get; set; }

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
