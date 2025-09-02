using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short unit type information
    /// </summary>
    public class UnitTypeShortEntry
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
    }
}