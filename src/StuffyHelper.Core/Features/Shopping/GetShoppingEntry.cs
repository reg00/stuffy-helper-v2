using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Shopping
{
    public class GetShoppingEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime ShoppingDate { get; set; }
        public string Check { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public EventShortEntry? Event { get; set; }
        [Required]
        public ParticipantShortEntry? Participant { get; set; }
        public List<PurchaseShortEntry> Purchases { get; set; }

        public GetShoppingEntry(ShoppingEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            ShoppingDate = entry.ShoppingDate;
            Check = entry.Check;
            Description = entry.Description;

            Event = new EventShortEntry(entry.Event);
            Participant = new ParticipantShortEntry(entry.Participant);
            Purchases = entry.Purchases.Select(x => new PurchaseShortEntry(x)).ToList();
        }
    }
}
