using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for creating or updating purchase tag information
    /// </summary>
    public class UpsertPurchaseTagEntry
    {
        /// <summary>
        /// Name of the purchase tag
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}