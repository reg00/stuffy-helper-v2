using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.Shopping
{
    public class GetShoppingEntry
    {
        public Guid Id { get; set; }
        public DateTime ShoppingDate { get; set; }
        public string Check { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public GetEventEntry? Event { get; set; }
        public GetParticipantEntry? Participant { get; set; }
        public List<GetPurchaseEntry> Purchases { get; set; }

        public GetShoppingEntry()
        {
            Purchases = new List<GetPurchaseEntry>();
        }

        public GetShoppingEntry(ShoppingEntry entry, bool includeEvent, bool includeParticipant, bool includePurchases)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            ShoppingDate = entry.ShoppingDate;
            Check = entry.Check;
            Description = entry.Description;
            IsActive = entry.IsActive;

            Event = includeEvent ? new GetEventEntry(entry.Event, null, false, false) : null;
            Participant = includeParticipant ? new GetParticipantEntry(entry.Participant, null, false, false, false) : null;
            Purchases = includePurchases ? entry.Purchases.Select(x => new GetPurchaseEntry(x, false, false)).ToList() : new List<GetPurchaseEntry>();
        }
    }
}
