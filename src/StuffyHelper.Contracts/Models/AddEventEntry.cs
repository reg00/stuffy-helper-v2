using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for creating new event
    /// </summary>
    public class AddEventEntry
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Event date start
        /// </summary>
        [Required]
        public DateTime EventDateStart { get; set; }
        
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = string.Empty;
        
        /// <summary>
        /// Event date end
        /// Optional
        /// </summary>
        public DateTime? EventDateEnd { get; set; }
    }
}
