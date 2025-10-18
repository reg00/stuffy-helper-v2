using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models{
    /// <summary>
    /// Model for creating or updating participant information
    /// </summary>
    public class UpsertParticipantEntry
    {
        /// <summary>
        /// Identifier of the event to associate with the participant
        /// </summary>
        //[Required]
        //public Guid EventId { get; init; }
        
        /// <summary>
        /// Identifier of the user to add as a participant
        /// </summary>
        [Required]
        public string UserId { get; init; } = string.Empty;
    }
}