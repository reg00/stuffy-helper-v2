using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public Guid EventId { get; set; }

        public virtual EventEntry Event { get; set; } = new();
        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertParticipantEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            EventId = entry.EventId;
        }
    }
}
