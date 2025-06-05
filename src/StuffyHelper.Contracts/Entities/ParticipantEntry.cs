using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    public class ParticipantEntry
    {
        public Guid Id { get; init; }
        public string UserId { get; init; } = string.Empty;
        public Guid EventId { get; set; }

        public virtual EventEntry Event { get; init; }
        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertParticipantEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            EventId = entry.EventId;
        }
    }
}
