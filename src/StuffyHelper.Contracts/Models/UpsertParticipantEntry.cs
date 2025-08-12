using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models{
    public class UpsertParticipantEntry
    {
        [Required]
        public Guid EventId { get; init; }
        [Required]
        public string UserId { get; init; } = string.Empty;
    }
}
