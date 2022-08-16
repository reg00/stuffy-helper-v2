using EnsureThat;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class GetPurchaseUsageEntry
    {
        public Guid Id { get; set; }
        public GetParticipantEntry? Participant { get; set; }
        public GetPurchaseEntry? Purchase { get; set; }

        public GetPurchaseUsageEntry()
        {

        }

        public GetPurchaseUsageEntry(PurchaseUsageEntry entry, bool includeParticipant, bool includePurchase)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Participant = includeParticipant ? new GetParticipantEntry(entry.Participant, null, false, false, false) : null;
            Purchase = includePurchase ? new GetPurchaseEntry(entry.Purchase, false, false, false, false) : null;
        }
    }
}
