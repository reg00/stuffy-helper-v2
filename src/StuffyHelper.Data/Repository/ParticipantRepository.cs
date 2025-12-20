using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Data.Storage;

namespace StuffyHelper.Data.Repository
{
    /// <inheritdoc />
    public class ParticipantRepository : IParticipantRepository
    {
        private readonly StuffyHelperContext _context;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ParticipantRepository(StuffyHelperContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<ParticipantEntry> GetParticipantAsync(Guid eventId, Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            try
            {
                var entry = await _context.Participants
                    .FirstOrDefaultAsync(e => e.Id == participantId && e.EventId == eventId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException("Participant {ParticipantId} not found.", participantId);
                return entry;
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }

        }

        /// <inheritdoc />
        public async Task<Response<ParticipantEntry>> GetParticipantsAsync(
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Participants
                    .Where(e => (string.IsNullOrWhiteSpace(userId) || e.UserId.ToLower().Equals(userId.ToLower())) &&
                     e.EventId == eventId)
                    .OrderByDescending(e => e.Event.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<ParticipantEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<ParticipantEntry> AddParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            try
            {
                var entry = await _context.Participants.AddAsync(participant, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException("Participant with User: {UserId} for Event: {EventId} already exists", ex, participant.UserId, participant.EventId);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task DeleteParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            try
            {
                _context.Participants.Remove(participant);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        /// <inheritdoc />
        public async Task<ParticipantEntry> UpdateParticipantAsync(ParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            try
            {
                var entry = _context.Participants.Update(participant);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException("Participant with User: {UserId} for Event: {EventId} already exists", ex, participant.UserId, participant.EventId);

                else throw new DbStoreException(ex);
            }
            catch (EntityNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }
    }
}
