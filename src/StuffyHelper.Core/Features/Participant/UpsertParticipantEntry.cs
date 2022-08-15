namespace StuffyHelper.Core.Features.Participant
{
    public class UpsertParticipantEntry
    {
        public Guid EventId { get; set; }
        public bool IsActive { get; set; }
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
