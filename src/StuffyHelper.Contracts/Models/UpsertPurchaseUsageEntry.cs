using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column(TypeName = "decimal(18,2)")]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Amount { get; init; }
    }
}