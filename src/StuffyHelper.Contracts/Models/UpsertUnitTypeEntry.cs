using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for creating or updating unit type information
    /// </summary>
    public class UpsertUnitTypeEntry
    {
        /// <summary>
        /// Name of the unit type
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}