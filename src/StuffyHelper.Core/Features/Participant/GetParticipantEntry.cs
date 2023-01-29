using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Purchase;
using StuffyHelper.Core.Features.PurchaseUsage;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Participant
{
    public class GetParticipantEntry
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public GetUserEntry? User { get; set; }
        [Required]
        public EventShortEntry? Event { get; set; }
        public List<PurchaseShortEntry> Purchases { get; set; }
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; set; }


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
