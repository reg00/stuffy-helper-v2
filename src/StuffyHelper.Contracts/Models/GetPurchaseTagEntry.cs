using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed purchase tag information
    /// </summary>
    public class GetPurchaseTagEntry
    {
        /// <summary>
        /// Purchase tag id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Name of the purchase tag
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// List of purchases associated with this tag
        /// </summary>
        public List<PurchaseShortEntry> Purchases { get; init; }
    }
}