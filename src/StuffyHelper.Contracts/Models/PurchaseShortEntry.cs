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
        /// Event id
        /// </summary>
        [Required]
        public Guid EventId { get; init; }
        
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
        /// Indicates whether the purchase is fully used/consumed
        /// </summary>
        [Required]
        public bool IsComplete { get; init; }
    }
}