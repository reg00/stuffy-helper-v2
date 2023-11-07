using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class GetPurchaseUsageEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public ParticipantShortEntry? Participant { get; set; }
        [Required]
        public PurchaseShortEntry? Purchase { get; set; }
        public int Amount { get; set; }

        public GetPurchaseUsageEntry()
        {

        }

        public GetPurchaseUsageEntry(PurchaseUsageEntry entry, UserShortEntry user)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));
            EnsureArg.IsNotNull(user, nameof(user));

            Id = entry.Id;
            Amount = entry.Amount;
            Participant = new ParticipantShortEntry(entry.Participant, user);
            Purchase = new PurchaseShortEntry(entry.Purchase);
        }
    }
}
