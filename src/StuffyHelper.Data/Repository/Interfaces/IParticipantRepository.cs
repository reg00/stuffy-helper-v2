using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Data.Repository.Interfaces
{
    /// <summary>
    /// Interface for work with participant repository
    /// </summary>
    public interface IParticipantRepository
    {
        /// <summary>
        /// Get participant by identifier
        /// </summary>
        /// <param name="participantId">Participant identifier</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Participant entry</returns>
        Task<ParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken);

        /// <summary>
        /// Get filtered list of participants
        /// </summary>
        /// <param name="offset">Number of records to skip</param>
        /// <param name="limit">Maximum number of records to return</param>
        /// <param name="eventId">Event identifier filter</param>
        /// <param name="userId">User identifier filter</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Paginated list of participants</returns>
        Task<Response<ParticipantEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Add new participant
        /// </summary>
        /// <param name="participant">Participant data to add</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Created participant entry</returns>
        Task<ParticipantEntry> AddParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);

        /// <summary>
        /// Delete participant
        /// </summary>
        /// <param name="participant">Participant entry to delete</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DeleteParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);

        /// <summary>
        /// Update participant information
        /// </summary>
        /// <param name="participant">Updated participant data</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Updated participant entry</returns>
        Task<ParticipantEntry> UpdateParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default);
    }
}