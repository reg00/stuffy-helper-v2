using EnsureThat;
using StuffyHelper.Core.Features.Checkout;
using StuffyHelper.Core.Features.Debt;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.Event
{
    public class EventEntry
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }
        public string UserId { get; init; } = string.Empty;
        public Uri? ImageUri { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }

        public virtual List<CheckoutEntry> Checkouts { get; set; } = new List<CheckoutEntry>();
        public virtual List<ParticipantEntry> Participants { get; set; } = new List<ParticipantEntry>();
        public virtual List<PurchaseEntry> Purchases { get; init; } = new List<PurchaseEntry>();
        public virtual List<MediaEntry> Medias { get; set; } = new List<MediaEntry>();
        public virtual List<DebtEntry> Debts { get; set; } = new List<DebtEntry>();

        public EventEntry()
        {
        }

        public EventEntry(
            string name,
            string description,
            DateTime eventDateStart,
            DateTime? eventDateEnd,
            string userId)
        {
            Name = name;
            Description = description;
            EventDateStart = eventDateStart;
            EventDateEnd = eventDateEnd;
            UserId = userId;
            CreatedDate = DateTime.UtcNow;
            IsCompleted = false;
            IsActive = true;
        }

        public void PatchFrom(UpdateEventEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Description = entry.Description;
            EventDateEnd = entry.EventDateEnd;
            EventDateStart = entry.EventDateStart;
        }
    }
}
