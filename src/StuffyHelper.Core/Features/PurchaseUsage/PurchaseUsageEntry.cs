using EnsureThat;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageEntry
    {
        public Guid Id { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid PurchaseId { get; set; }
        public double Amount { get; set; }

        public virtual ParticipantEntry Participant { get; set; }
        public virtual PurchaseEntry Purchase { get; set; }

        public void PatchFrom(UpsertPurchaseUsageEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            ParticipantId = entry.ParticipantId;
            PurchaseId = entry.PurchaseId;
            Amount = entry.Amount;
        }
    }
}
