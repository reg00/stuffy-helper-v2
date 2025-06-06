using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class GetParticipantEntry
    {
        [Required]
        public Guid Id { get; init; }

        [Required]
        public GetUserEntry? User { get; init; }
        [Required]
        public EventShortEntry? Event { get; init; }
        public List<PurchaseShortEntry> Purchases { get; init; }
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; init; }


        public GetParticipantEntry()
        {
            Purchases = new List<PurchaseShortEntry>();
            PurchaseUsages = new List<PurchaseUsageShortEntry>();
        }

        public GetParticipantEntry(
            ParticipantEntry entry,
            GetUserEntry user,
            IEnumerable<PurchaseUsageShortEntry> purchaseUsages)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            User = user;
            Event = new EventShortEntry(entry.Event);
            Purchases = entry.Purchases.Select(x => new PurchaseShortEntry(x)).ToList();
            PurchaseUsages = purchaseUsages;
        }
    }
}
