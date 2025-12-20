using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models{
    /// <summary>
    /// Model for updating purchase information
    /// </summary>
    public class UpdatePurchaseEntry
    {
        /// <summary>
        /// Name of the purchase
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Cost of the purchase
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Cost { get; init; }
        
        /// <summary>
        /// Purchase usages
        /// </summary>
        [Required]
        public UpsertPurchaseUsageEntry[] PurchaseUsages { get; set; } = Array.Empty<UpsertPurchaseUsageEntry>();
    }
}