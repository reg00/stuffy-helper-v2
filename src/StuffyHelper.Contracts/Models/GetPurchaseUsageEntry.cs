using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed purchase usage information
    /// </summary>
    public class GetPurchaseUsageEntry
    {
        /// <summary>
        /// Purchase usage id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Participant who used the purchase
        /// </summary>
        [Required]
        public ParticipantShortEntry? Participant { get; init; }
        
        /// <summary>
        /// Purchase that was used
        /// </summary>
        [Required]
        public PurchaseShortEntry? Purchase { get; init; }
        
        /// <summary>
        /// Amount of the purchase that was used
        /// </summary>
        public double Amount { get; init; }
    }
}