using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        /// Amount/quantity of the purchase
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Amount { get; init; }
        
        /// <summary>
        /// Indicates whether the purchase can be partially used
        /// </summary>
        [Required]
        public bool IsPartial { get; init; }

        /// <summary>
        /// List of tags associated with the purchase
        /// </summary>
        public List<PurchaseTagShortEntry> PurchaseTags { get; init; } = new();
        
        /// <summary>
        /// Identifier of the unit type for the purchase amount
        /// </summary>
        [Required]
        public Guid UnitTypeId { get; init; }
    }
}