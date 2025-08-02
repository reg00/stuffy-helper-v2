using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
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

        Task DeleteParticipantAsync(string token, Guid participantId, CancellationToken cancellationToken = default);
    }
}
