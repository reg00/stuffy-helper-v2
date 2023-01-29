using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageShortEntry
    {
        [Required]
        public Guid PurchaseUsageId { get; set; }
        public Guid ParticipantId { get; set; }

        public PurchaseUsageShortEntry(PurchaseUsageEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            PurchaseUsageId = entry.Id;
            ParticipantId = entry.Participant.Id;
        }
    }
}
