using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Purchase entity
    /// </summary>
    public class PurchaseEntry
    {
        /// <summary>
        /// Identifier of purchase
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// Purchase name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        
        /// <summary>
        /// Purchase cost
        /// </summary>
        public long Cost { get; set; }
        
        /// <summary>
        /// Participant id
        /// </summary>
        public Guid ParticipantId { get; init; }
        
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Is completed
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Linked purchase usages 
        /// </summary>
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new();
        /// <summary>
        /// Linked event
        /// </summary>
        public virtual EventEntry Event { get; init; }
        /// <summary>
        /// Linked participant owner
        /// </summary>
        public virtual ParticipantEntry Owner { get; init; }

        public void PatchFrom(UpdatePurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Cost = entry.Cost;
        }
    }
}
