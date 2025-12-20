using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for updating event information
    /// </summary>
    public class UpdateEventEntry
    {
        /// <summary>
        /// Name of the event
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Description of the event
        /// </summary>
        public string Description { get; init; } = string.Empty;
        
        /// <summary>
        /// Event start date and time
        /// </summary>
        [Required]
        public DateTime EventDateStart { get; set; }
        
        /// <summary>
        /// Event end date and time (if specified)
        /// </summary>
        public DateTime? EventDateEnd { get; set; }
    }
}