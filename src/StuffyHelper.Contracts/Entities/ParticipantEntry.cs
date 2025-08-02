using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Participant entity
    /// </summary>
    public class ParticipantEntry
    {
        /// <summary>
        /// Identifier of participant
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; init; } = string.Empty;
        
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }

        /// <summary>
        /// Linked event
        /// </summary>
        public virtual EventEntry Event { get; init; }
        /// <summary>
        /// Linked purchases
        /// </summary>
        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();
        /// <summary>
        /// Linked purchase usages
        /// </summary>
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();


        public void PatchFrom(UpsertParticipantEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            EventId = entry.EventId;
        }
    }
}
