using System.ComponentModel.DataAnnotations;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed event description
    /// </summary>
    public class GetEventEntry
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
        /// Date when event was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; init; }
        
        /// <summary>
        /// Event start date and time
        /// </summary>
        [Required]
        public DateTime EventDateStart { get; init; }
        
        /// <summary>
        /// Event end date and time (if specified)
        /// </summary>
        public DateTime? EventDateEnd { get; init; }
        
        /// <summary>
        /// Indicates whether the event is completed
        /// </summary>
        [Required]
        public bool IsCompleted { get; init; }
        
        /// <summary>
        /// Main media URI for the event
        /// </summary>
        public Uri? MediaUri { get; init; }

        /// <summary>
        /// Event creator information
        /// </summary>
        [Required]
        public UserShortEntry? User { get; init; }
        
        /// <summary>
        /// List of event participants
        /// </summary>
        public List<ParticipantShortEntry> Participants { get; init; } = new();
        
        /// <summary>
        /// List of purchases for the event
        /// </summary>
        public List<PurchaseShortEntry> Purchases { get; init; } = new();
        
        /// <summary>
        /// List of media files associated with the event
        /// </summary>
        public List<MediaShortEntry> Medias { get; init; } = new();
    }
}