using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for creating or updating purchase usage information
    /// </summary>
    public class UpsertPurchaseUsageEntry
    {
        /// <summary>
        /// Identifier of the purchase being used
        /// </summary>
        [Required]
        public Guid PurchaseId { get; init; }
        
        /// <summary>
        /// Identifier of the participant using the purchase
        /// </summary>
        [Required]
        public Guid ParticipantId { get; init; }
        
        /// <summary>
        /// Amount of the purchase being used
        /// </summary>
        [Required]
        public long Amount { get; init; }
    }
}