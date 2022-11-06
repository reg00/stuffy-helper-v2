using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Participant
{
    public class UpsertParticipantEntry
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string UserId { get; set; }

        public ParticipantEntry ToCommonEntry()
        {
            return new ParticipantEntry()
            {
                EventId = EventId,
                UserId = UserId,
                IsActive = true
            };
        }
    }
}
