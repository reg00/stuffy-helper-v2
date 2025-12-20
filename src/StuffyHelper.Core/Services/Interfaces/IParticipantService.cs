using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for work with participants
    /// </summary>
    public interface IParticipantService
    {
        /// <summary>
        /// Get participant by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="participantId">Participant identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Detailed participant information</returns>
        Task<GetParticipantEntry> GetParticipantAsync(Guid eventId, Guid participantId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of participants
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="userId">User identifier filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of participants</returns>
        Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? userId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new participant
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="participant">Participant data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created participant information</returns>
        Task<ParticipantShortEntry> AddParticipantAsync(Guid eventId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete participant by identifier
        /// </summary>
        /// <param name="eventId">Event identifier</param>
        /// <param name="userId">User identifier for authorization check</param>
        /// <param name="participantId">Participant identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteParticipantAsync(string userId, Guid eventId, Guid participantId, CancellationToken cancellationToken = default);
    }
}