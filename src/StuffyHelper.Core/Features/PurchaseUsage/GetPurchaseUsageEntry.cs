using EnsureThat;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class GetPurchaseUsageEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public ParticipantShortEntry? Participant { get; init; }
        [Required]
        public PurchaseShortEntry? Purchase { get; init; }
        public double Amount { get; init; }

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
