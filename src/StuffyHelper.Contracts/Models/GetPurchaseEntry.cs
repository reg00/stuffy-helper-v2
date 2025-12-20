using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed purchase information
    /// </summary>
    public class GetPurchaseEntry
    {
        /// <summary>
        /// Purchase id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Name of the purchase
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Cost of the purchase
        /// </summary>
        [Required]
        public long Cost { get; init; }
        
        /// <summary>
        /// Indicates whether the purchase is completed
        /// </summary>
        [Required]
        public bool IsComplete { get; init; }

        /// <summary>
        /// Event Id associated with the purchase
        /// </summary>
        [Required]
        public Guid EventId { get; init; }
        
        /// <summary>
        /// List of usage records for this purchase
        /// </summary>
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; init; }
        
        /// <summary>
        /// Participant who made the purchase
        /// </summary>
        public ParticipantShortEntry Participant { get; init; }
    }
}