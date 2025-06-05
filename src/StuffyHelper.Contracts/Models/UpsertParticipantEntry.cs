using System.ComponentModel.DataAnnotations;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models{
    public class UpsertParticipantEntry
    {
        [Required]
        public Guid EventId { get; init; }
        [Required]
        public string UserId { get; init; } = string.Empty;

        public ParticipantEntry ToCommonEntry()
        {
            return new ParticipantEntry()
            {
                EventId = EventId,
                UserId = UserId
            };
        }
    }
}
