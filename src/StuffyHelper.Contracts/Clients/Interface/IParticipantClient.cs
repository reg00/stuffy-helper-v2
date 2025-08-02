using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IParticipantClient
{
    public Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        string? userId = null,
        CancellationToken cancellationToken = default);

    public Task<GetParticipantEntry> GetParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default);

    public Task<ParticipantShortEntry> CreateParticipantAsync(
        string token,
        UpsertParticipantEntry body,
        CancellationToken cancellationToken = default);

    public Task DeleteParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default);
}