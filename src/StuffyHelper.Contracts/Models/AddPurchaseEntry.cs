using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for create new purchase
    /// </summary>
    public class AddPurchaseEntry
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Cost
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Cost { get; init; }
        
        /// <summary>
        /// Participant id
        /// </summary>
        [Required]
        public Guid ParticipantId { get; init; }

        /// <summary>
        /// Purchase usages
        /// </summary>
        [Required]
        public UpsertPurchaseUsageEntry[] PurchaseUsages { get; set; } = Array.Empty<UpsertPurchaseUsageEntry>();
    }
}
