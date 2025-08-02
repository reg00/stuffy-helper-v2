using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Event entity
    /// </summary>
    public class EventEntry
    {
        /// <summary>
        /// Identifier of event
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// Event name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Event description
        /// </summary>
        public string Description { get; set; } = string.Empty;
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Date when event start
        /// </summary>
        public DateTime EventDateStart { get; set; }
        
        /// <summary>
        /// Date when event end
        /// </summary>
        public DateTime? EventDateEnd { get; set; }
        
        /// <summary>
        /// Owner user id
        /// </summary>
        public string UserId { get; init; } = string.Empty;
        
        /// <summary>
        /// Avatar uri
        /// </summary>
        public Uri? ImageUri { get; set; }
        
        /// <summary>
        /// Is completed
        /// </summary>
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Linked checkouts
        /// </summary>
        public virtual List<CheckoutEntry> Checkouts { get; set; } = new List<CheckoutEntry>();
        /// <summary>
        /// Linked participants
        /// </summary>
        public virtual List<ParticipantEntry> Participants { get; set; } = new List<ParticipantEntry>();
        /// <summary>
        /// Linked purchases
        /// </summary>
        public virtual List<PurchaseEntry> Purchases { get; init; } = new List<PurchaseEntry>();
        /// <summary>
        /// Linked medias
        /// </summary>
        public virtual List<MediaEntry> Medias { get; set; } = new List<MediaEntry>();
        /// <summary>
        /// Linked debts
        /// </summary>
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
