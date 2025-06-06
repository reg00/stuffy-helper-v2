using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    public interface IParticipantRepository
    {
        Task<ParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken);

        Task<Response<ParticipantEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default);

        Task<ParticipantEntry> AddParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);

        Task DeleteParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);

        Task<ParticipantEntry> UpdateParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);
    }
}
