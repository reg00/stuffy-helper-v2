using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short purchase tag information
    /// </summary>
    public class PurchaseTagShortEntry
    {
        /// <summary>
        /// Purchase tag id
        /// </summary>
        [Required] public Guid Id { get; init; }
        
        /// <summary>
        /// Name of the purchase tag
        /// </summary>
        [Required] public string Name { get; init; } = string.Empty;
    }
}