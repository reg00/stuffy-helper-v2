using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed unit type information
    /// </summary>
    public class GetUnitTypeEntry
    {
        /// <summary>
        /// Unit type id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Name of the unit type
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;

        /// <summary>
        /// List of purchases using this unit type
        /// </summary>
        public List<PurchaseShortEntry> Purchases { get; init; }
    }
}