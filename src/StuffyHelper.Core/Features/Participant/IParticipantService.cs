using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Participant
{
    public interface IParticipantService
    {
        Task<GetParticipantEntry> GetParticipantAsync(string token, Guid participantId, CancellationToken cancellationToken);

        Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            string token,
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default);

        Task<ParticipantShortEntry> AddParticipantAsync(string token, UpsertParticipantEntry participant, CancellationToken cancellationToken = default);

        Task DeleteParticipantAsync(string userId, Guid participantId, CancellationToken cancellationToken = default);

        Task<ParticipantShortEntry> UpdateParticipantAsync(string token, Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default);
    }
}
