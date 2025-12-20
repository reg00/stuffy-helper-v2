using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Clients.Interface;

/// <summary>
/// Interface for work with participants
/// </summary>
public interface IParticipantClient
{
    /// <summary>
    /// Return list of participants from event
    /// </summary>
    public Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
        string token,
        Guid eventId,
        int offset = 0,
        int limit = 10,
        string? userId = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Return participant by id
    /// </summary>
    public Task<GetParticipantEntry> GetParticipantAsync(
        string token,
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Add new participant into event
    /// </summary>
    public Task<ParticipantShortEntry> CreateParticipantAsync(
        string token,
        Guid eventId,
        UpsertParticipantEntry body,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove participant from event
    /// </summary>
    public Task DeleteParticipantAsync(
        string token,
        Guid eventId,
        Guid participantId,
        CancellationToken cancellationToken = default);
}