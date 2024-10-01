using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Participant
{
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
