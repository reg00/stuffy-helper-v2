using System.ComponentModel.DataAnnotations;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed participant information
    /// </summary>
    public class GetParticipantEntry
    {
        /// <summary>
        /// Participant id
        /// </summary>
        [Required]
        public Guid Id { get; init; }

        /// <summary>
        /// User information of the participant
        /// </summary>
        [Required]
        public GetUserEntry? User { get; init; }
        
        /// <summary>
        /// Event information that the participant is associated with
        /// </summary>
        [Required]
        public EventShortEntry? Event { get; init; }
        
        /// <summary>
        /// List of purchases made by the participant
        /// </summary>
        public List<PurchaseShortEntry> Purchases { get; init; } = new();
        
        /// <summary>
        /// List of purchase usages by the participant
        /// </summary>
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; init; } = new List<PurchaseUsageShortEntry>();
    }
}