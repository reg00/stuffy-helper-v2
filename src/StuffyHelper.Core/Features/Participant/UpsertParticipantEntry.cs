namespace StuffyHelper.Core.Features.Participant
{
    public class UpsertParticipantEntry
    {
        public Guid EventId { get; set; }

        public ParticipantEntry ToCommonEntry(string userId)
        {
            return new ParticipantEntry()
            {
                EventId = EventId,
                UserId = userId,
            };
        }
    }
}
