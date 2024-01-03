using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.EntityFrameworkCore.Features.Schema;

namespace StuffyHelper.EntityFrameworkCore.Features.Storage
{
    public class EfEventStore : IEventStore
    {
        private readonly StuffyHelperContext _context;

        public EfEventStore(StuffyHelperContext context)
        {
            _context = context;
        }

        public async Task<EventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _context.Events
                    .FirstOrDefaultAsync(e =>
                    ((e.Id == eventId) &&
                    (string.IsNullOrWhiteSpace(userId) || e.Participants.Any(x => x.UserId == userId))),
                    cancellationToken);

                if (entry is null)
                    throw new EntityNotFoundException($"Event with Id '{eventId}' Not Found.");

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

        public async Task<PagedData<EventEntry>> GetEventsAsync(
            int offset = 0,
            int limit = 10,
            string? name = null,
            string? description = null,
            DateTime? createdDateStart = null,
            DateTime? createdDateEnd = null,
            DateTime? eventDateStartMin = null,
            DateTime? eventDateStartMax = null,
            DateTime? eventDateEndMin = null,
            DateTime? eventDateEndMax = null,
            string? userId = null,
            bool? isCompleted = null,
            bool? isActive = null,
            Guid? participantId = null,
            Guid? purchaseId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Events
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.ToLower().Contains(name.ToLower())) &&
                    (string.IsNullOrWhiteSpace(description) || e.Description.ToLower().Contains(description.ToLower())) &&
                    (createdDateStart == null || createdDateStart.Value >= e.CreatedDate) &&
                    (createdDateEnd == null || createdDateEnd.Value <= e.CreatedDate) &&
                    (eventDateStartMin == null || eventDateStartMin.Value >= e.EventDateStart) &&
                    (eventDateStartMax == null || eventDateStartMax.Value <= e.EventDateStart) &&
                    (eventDateEndMin == null || eventDateEndMin.Value >= e.EventDateEnd) &&
                    (eventDateEndMax == null || eventDateEndMax.Value <= e.EventDateEnd) &&
                    //(string.IsNullOrWhiteSpace(userId) || e.UserId.ToLower() == userId.ToLower()) &&
                    (string.IsNullOrWhiteSpace(userId) || e.Participants.Any(x => x.UserId == userId)) &&
                    (isCompleted == null || isCompleted == e.IsCompleted) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (participantId == null || e.Participants.Any(x => x.Id == participantId)) &&
                    (purchaseId == null || e.Purchases.Any(x => x.Id == purchaseId)))
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new PagedData<EventEntry>()
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

        public async Task<EventEntry> AddEventAsync(EventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));

            try
            {
                @event.CreatedDate = DateTime.UtcNow;
                var entry = await _context.Events.AddAsync(@event, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Event with name '{@event.Name}' and event date '{@event.EventDateStart}' already exists", ex);

                else throw new DbStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var @event = await _context.Events
                    .FirstOrDefaultAsync(
                    s => s.Id == eventId, cancellationToken);

                if (@event is null)
                {
                    throw new EntityNotFoundException($"Event with Id '{eventId}' not found.");
                }

                @event.IsActive = false;

                _context.Events.Update(@event);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DbStoreException(ex);
            }
        }

        public async Task<EventEntry> UpdateEventAsync(EventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));

            try
            {
                var entry = _context.Events.Update(@event);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Event with name '{@event.Name}' and event date '{@event.EventDateStart}' already exists", ex);

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
