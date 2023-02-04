using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfParticipantStore : IParticipantStore
    {
        private readonly StuffyHelperContext _context;

        public EfParticipantStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<ParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            try
            {
                var entry = await _context.Participants
                    .FirstOrDefaultAsync(e => e.Id == participantId, cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"Participant with Id '{participantId}' Not Found.");

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

        public async Task<Response<ParticipantEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Participants
                    .Where(e => (string.IsNullOrWhiteSpace(userId) || e.UserId.ToLower().Equals(userId.ToLower())) &&
                    (eventId == null || e.EventId == eventId))
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
                    throw new EntityAlreadyExistsException($"Participant with userId '{participant.UserId}' and eventId '{participant.EventId}' already exists", ex);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeleteParticipantAsync(Guid participantId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            try
            {
                var participant = await _context.Participants
                    .FirstOrDefaultAsync(
                    s => s.Id == participantId, cancellationToken);

                if (participant is null)
                {
                    throw new EntityNotFoundException($"Participant with Id '{participantId}' not found.");
                }

                _context.Participants.Remove(participant);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

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
                    throw new EntityAlreadyExistsException($"Participant with userId '{participant.UserId}' and eventId '{participant.EventId}' already exists", ex);

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
