using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.PurchaseUsage;
using StuffyHelper.Core.Features.Shopping;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Participant
{
    public class GetParticipantEntry
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public UserShortEntry? User { get; set; }
        [Required]
        public EventShortEntry? Event { get; set; }
        public List<ShoppingShortEntry> Shoppings { get; set; }
        public List<PurchaseUsageShortEntry> PurchaseUsages { get; set; }


        public GetParticipantEntry()
        {
            Shoppings = new List<ShoppingShortEntry>();
            PurchaseUsages = new List<PurchaseUsageShortEntry>();
        }

        public GetParticipantEntry(
            ParticipantEntry entry,
            UserShortEntry user,
            List<PurchaseUsageShortEntry> purchaseUsages = null)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            User = user;
            Event = new EventShortEntry(entry.Event);
            Shoppings = entry.Shoppings.Select(x => new ShoppingShortEntry(x)).ToList();
            PurchaseUsages = purchaseUsages;
        }
    }
}
