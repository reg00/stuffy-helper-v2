using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.Shopping
{
    public class ShoppingEntry
    {
        public Guid Id { get; set; }
        public DateTime ShoppingDate { get; set; }
        public Guid ParticipantId { get; set; }
        public Guid EventId { get; set; }
        public string Check { get; set; }
        public bool IsActive { get; set; }

        public virtual EventEntry Event { get; set; }
        public virtual ParticipantEntry Participant { get; set; }
        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();

        public void PatchFrom(UpsertShoppingEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            ShoppingDate = entry.ShoppingDate;
            ParticipantId = entry.ParticipantId;
            EventId = entry.EventId;
            Check = entry.Check;
            IsActive = entry.IsActive;
        }

    }
}
