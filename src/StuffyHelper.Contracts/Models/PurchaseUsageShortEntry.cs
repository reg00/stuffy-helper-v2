using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
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
