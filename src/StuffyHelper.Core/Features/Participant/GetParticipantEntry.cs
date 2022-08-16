using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Features.Participant
{
    public class GetParticipantEntry
    {
        public Guid Id { get; set; }
        public bool IsActive { get; set; }

        public GetUserEntry? User { get; set; }
        public GetEventEntry? Event { get; set; }
        public List<GetShoppingEntry> Shoppings { get; set; }
        public List<GetPurchaseUsageEntry> PurchaseUsages { get; set; }


        public GetParticipantEntry()
        {
            Shoppings = new List<GetShoppingEntry>();
            PurchaseUsages = new List<GetPurchaseUsageEntry>();
        }

        public GetParticipantEntry(ParticipantEntry entry, GetUserEntry user, bool includeEvent, bool includeShoppings, bool includePurchaseUsages)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));
            //EnsureArg.IsNotNull(user, nameof(user));

            Id = entry.Id;
            IsActive = entry.IsActive;
            User = user;
            Event = includeEvent ? new GetEventEntry(entry.Event, user, false, false) : null;
            Shoppings = includeShoppings ? entry.Shoppings.Select(x => new GetShoppingEntry(x, false, false, false)).ToList() : new List<GetShoppingEntry>();
            PurchaseUsages = includePurchaseUsages ? entry.PurchaseUsages.Select(x => new GetPurchaseUsageEntry(x, false, false)).ToList() : new List<GetPurchaseUsageEntry>();
        }
    }
}
