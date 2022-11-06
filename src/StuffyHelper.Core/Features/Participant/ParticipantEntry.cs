using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public Guid EventId { get; set; }
        public bool IsActive { get; set; }

        public virtual EventEntry Event { get; set; }
        public virtual List<ShoppingEntry> Shoppings { get; set; } = new List<ShoppingEntry>();
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertParticipantEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            EventId = entry.EventId;
        }
    }
}
