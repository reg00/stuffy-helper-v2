using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Participant
{
    public interface IParticipantService
    {
        Task<GetParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken);

        Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string userId = null,
            CancellationToken cancellationToken = default);

        Task<ParticipantShortEntry> AddParticipantAsync(UpsertParticipantEntry participant, CancellationToken cancellationToken = default);

        Task DeleteParticipantAsync(Guid participantId, CancellationToken cancellationToken = default);

        Task<ParticipantShortEntry> UpdateParticipantAsync(Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default);
    }
}
