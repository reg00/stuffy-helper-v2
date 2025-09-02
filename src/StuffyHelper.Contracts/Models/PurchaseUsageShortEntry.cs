using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short purchase usage information
    /// </summary>
    public class PurchaseUsageShortEntry
    {
        /// <summary>
        /// Purchase usage identifier
        /// </summary>
        [Required]
        public Guid PurchaseUsageId { get; init; }
        
        /// <summary>
        /// Purchase identifier
        /// </summary>
        public Guid PurchaseId { get; init; }
        
        /// <summary>
        /// Participant identifier
        /// </summary>
        public Guid ParticipantId { get; init; }
        
        /// <summary>
        /// Amount of the purchase that was used
        /// </summary>
        public double Amount { get; init; }
    }
}