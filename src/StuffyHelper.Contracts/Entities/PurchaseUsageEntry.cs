using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class PurchaseUsageEntry
    {
        /// <summary>
        /// Identifier of purchase usage
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Participant id
        /// </summary>
        public Guid ParticipantId { get; set; }
        /// <summary>
        /// Purchase id
        /// </summary>
        public Guid PurchaseId { get; set; }
        /// <summary>
        /// Checkout id
        /// </summary>
        public Guid? CheckoutId { get; set; }
        /// <summary>
        /// Amount
        /// </summary>
        public long Amount { get; set; }

        
        /// <summary>
        /// Linked participant
        /// </summary>
        public virtual ParticipantEntry Participant { get; init; }
        /// <summary>
        /// Linked purchase
        /// </summary>
        public virtual PurchaseEntry Purchase { get; init; }
        /// <summary>
        /// Linked checkout
        /// </summary>
        public virtual CheckoutEntry Checkout { get; set; }

        public void PatchFrom(UpsertPurchaseUsageEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            ParticipantId = entry.ParticipantId;
            PurchaseId = entry.PurchaseId;
            Amount = entry.Amount;
        }
    }
}
