using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short participant information
    /// </summary>
    public class ParticipantShortEntry
    {
        /// <summary>
        /// Participant id
        /// </summary>
        [Required]
        public Guid Id { get; init; }
        
        /// <summary>
        /// Participant name
        /// </summary>
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Participant image URI
        /// </summary>
        public Uri? ImageUri { get; init; }
    }
}