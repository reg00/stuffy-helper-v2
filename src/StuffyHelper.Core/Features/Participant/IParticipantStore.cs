using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Participant
{
    public interface IParticipantStore
    {
        Task<ParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken);

        Task<Response<ParticipantEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string userId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default);

        Task<ParticipantEntry> AddParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);

        Task DeleteParticipantAsync(Guid participantId, CancellationToken cancellationToken = default);

        Task<ParticipantEntry> UpdateParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);
    }
}
