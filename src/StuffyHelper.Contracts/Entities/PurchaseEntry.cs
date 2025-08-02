using EnsureThat;
using StuffyHelper.Contracts.Interfaces;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Purchase entity
    /// </summary>
    public class PurchaseEntry : ITaggableEntry
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
        public double Cost { get; set; }
        
        /// <summary>
        /// Purchase amount
        /// </summary>
        public double Amount { get; set; }
        
        /// <summary>
        /// Is purchase partial
        /// </summary>
        public bool IsPartial { get; set; }
        
        /// <summary>
        /// Unit type id
        /// </summary>
        public Guid? UnitTypeId { get; set; }
        
        /// <summary>
        /// Participant id
        /// </summary>
        public Guid ParticipantId { get; init; }
        
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; init; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedDate { get; set; }
        
        /// <summary>
        /// Is completed
        /// </summary>
        public bool IsComplete { get; set; }

        /// <summary>
        /// Linked purchase tags
        /// </summary>
        public virtual List<PurchaseTagEntry> PurchaseTags { get; set; } = new List<PurchaseTagEntry>();
        /// <summary>
        /// Linked unit type
        /// </summary>
        public virtual UnitTypeEntry? UnitType { get; init; }
        /// <summary>
        /// Linked purchase usages 
        /// </summary>
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; init; } = new List<PurchaseUsageEntry>();
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
            Amount = entry.Amount;
            IsPartial = entry.IsPartial;
            UnitTypeId = entry.UnitTypeId;
        }
    }
}
