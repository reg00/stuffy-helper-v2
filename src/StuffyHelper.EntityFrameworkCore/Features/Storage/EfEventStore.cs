﻿using EnsureThat;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using StuffyHelper.Core.Exceptions;
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

        public async Task<EventEntry> GetEventAsync(Guid eventId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _context.Events
                    .FirstOrDefaultAsync(e => e.Id == eventId, cancellationToken);

                if (entry is null)
                    throw new ResourceNotFoundException($"Event with Id '{eventId}' Not Found.");

                return entry;
            }
            catch (ResourceNotFoundException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }

        }

        public async Task<Response<EventEntry>> GetEventsAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            string description = null,
            DateTime? createdDateStart = null,
            DateTime? createdDateEnd = null,
            DateTime? eventDateStart = null,
            DateTime? eventDateEnd = null,
            string userId = null,
            bool? isCompleted = null,
            bool? isActive = null,
            Guid? participantId = null,
            Guid? shoppingId = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var searchedData = await _context.Events
                    .Where(e => (string.IsNullOrWhiteSpace(name) || e.Name.Contains(name, StringComparison.OrdinalIgnoreCase)) &&
                    (string.IsNullOrWhiteSpace(description) || e.Description.Contains(description, StringComparison.OrdinalIgnoreCase)) &&
                    (createdDateStart == null || createdDateStart.Value >= e.CreatedDate) &&
                    (createdDateEnd == null || createdDateEnd.Value <= e.CreatedDate) &&
                    (eventDateStart == null || eventDateStart.Value >= e.EventDate) &&
                    (eventDateEnd == null || eventDateEnd.Value <= e.EventDate) &&
                    (string.IsNullOrWhiteSpace(userId) || e.UserId.Contains(userId, StringComparison.OrdinalIgnoreCase)) &&
                    (isCompleted == null || isCompleted == e.IsCompleted) &&
                    (isActive == null || isActive == e.IsActive) &&
                    (participantId == null || e.Participants.Any(x => x.Id == participantId)) &&
                    (shoppingId == null || e.Shoppings.Any(x => x.Id == shoppingId)))
                    .OrderByDescending(e => e.CreatedDate)
                    .ToListAsync(cancellationToken);

                return new Response<EventEntry>()
                {
                    Data = searchedData.Skip(offset).Take(limit),
                    TotalPages = (int)Math.Ceiling(searchedData.Count() / (double)limit),
                    Total = searchedData.Count()
                };
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }

        public async Task<EventEntry> AddEventAsync(EventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));

            try
            {
                @event.CreatedDate = DateTime.Now;
                var entry = await _context.Events.AddAsync(@event, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                return entry.Entity;
            }
            catch (DbUpdateException ex)
            {
                if ((ex.InnerException as PostgresException)?.SqlState == "23505")
                    throw new EntityAlreadyExistsException($"Event with name '{@event.Name}' and event date '{@event.EventDate}' already exists", ex);

                else throw new DataStoreException(ex);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
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
                    throw new ResourceNotFoundException($"Event with Id '{eventId}' not found.");
                }

                @event.IsActive = false;

                _context.Events.Update(@event);
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
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
                    throw new EntityAlreadyExistsException($"Event with name '{@event.Name}' and event date '{@event.EventDate}' already exists", ex);

                else throw new DataStoreException(ex);
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataStoreException(ex);
            }
        }
    }
}