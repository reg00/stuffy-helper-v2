using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Event model for listing
    /// </summary>
    public class EventShortEntry
    {
        /// <summary>
        /// Event id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Description
        /// </summary>
        public string Description { get; init; } = string.Empty;
        
        /// <summary>
        /// Event date start
        /// </summary>
        [Required]
        public DateTime? EventDateStart { get; init; }
        
        /// <summary>
        /// Event date end
        /// Optional
        /// </summary>
        public DateTime? EventDateEnd { get; init;}
        
        /// <summary>
        /// Is event completed
        /// </summary>
        [Required]
        public bool IsCompleted { get; init; }
        
        /// <summary>
        /// Avatar image url
        /// </summary>
        public Uri? ImageUri { get; init; }
    }
}
