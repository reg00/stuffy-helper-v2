using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short purchase information
    /// </summary>
    public class PurchaseShortEntry
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
        public double Cost { get; init; }
        
        /// <summary>
        /// Amount/quantity of the purchase
        /// </summary>
        [Required]
        public double Amount { get; init; }
        
        /// <summary>
        /// Indicates whether the purchase can be partially used
        /// </summary>
        [Required]
        public bool IsPartial { get; init; }
        
        /// <summary>
        /// Indicates whether the purchase is fully used/consumed
        /// </summary>
        [Required]
        public bool IsComplete { get; init; }

        /// <summary>
        /// List of tags associated with the purchase
        /// </summary>
        public List<PurchaseTagShortEntry> PurchaseTags { get; init; }
        
        /// <summary>
        /// Unit type for the purchase amount
        /// </summary>
        public UnitTypeShortEntry? UnitType { get; init; }
    }
}