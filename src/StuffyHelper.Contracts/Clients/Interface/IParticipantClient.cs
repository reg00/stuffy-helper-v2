using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

public interface IParticipantClient
{
    /// <summary>
    /// Получение списка участников ивента
    /// </summary>
    public Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
        string token,
        int offset = 0,
        int limit = 10,
        Guid? eventId = null,
        string? userId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получение участника ивента по идентификатору
    /// </summary>
    public Task<GetParticipantEntry> GetParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Добавление участника
    /// </summary>
    public Task<ParticipantShortEntry> CreateParticipantAsync(
        string token,
        UpsertParticipantEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаление участника
    /// </summary>
    public Task DeleteParticipantAsync(
        string token,
        Guid participantId,
        CancellationToken cancellationToken = default);
}