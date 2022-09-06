using EnsureThat;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Event
{
    public class EventService : IEventService
    {
        private readonly IEventStore _eventStore;
        private readonly IAuthorizationService _authorizationService;

        public EventService(IEventStore eventStore, IAuthorizationService authorizationService)
        {
            _eventStore = eventStore;
            _authorizationService = authorizationService;
        }

        public async Task<GetEventEntry> GetEventAsync(Guid eventId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var entry = await _eventStore.GetEventAsync(eventId, cancellationToken);
            var user = await _authorizationService.GetUser(userId: entry.UserId);

            return new GetEventEntry(entry, new GetUserEntry(user), true, true);
        }

        public async Task<Response<GetEventEntry>> GetEventsAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            string description = null,
            DateTime? createdDateStart = null,
            DateTime? createdDateEnd = null,
            DateTime? eventDateStartMin = null,
            DateTime? eventDateStartMax = null,
            DateTime? eventDateEndMin = null,
            DateTime? eventDateEndMax = null,
            string userId = null,
            bool? isCompleted = null,
            bool? isActive = null,
            Guid? participantId = null,
            Guid? shoppingId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _eventStore.GetEventsAsync(offset, limit, name, description, createdDateStart,
                                                        createdDateEnd, eventDateStartMin, eventDateStartMax, eventDateEndMin , eventDateEndMax,
                                                        userId, isCompleted, isActive, participantId, shoppingId, cancellationToken);
            var events = new List<GetEventEntry>();

            foreach (var @event in resp.Data)
            {
                var user = await _authorizationService.GetUser(userId: @event.UserId);
                events.Add(new GetEventEntry(@event, new GetUserEntry(user), true, true));
            }

            return new Response<GetEventEntry>()
            {
                Data = events,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<GetEventEntry> AddEventAsync(UpsertEventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));

            var user = await _authorizationService.GetUser(userId: @event.UserId);
            var entry = @event.ToCommonEntry();
            var result = await _eventStore.AddEventAsync(entry, cancellationToken);

            return new GetEventEntry(result, new GetUserEntry(user), false, false);
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await _eventStore.DeleteEventAsync(eventId, cancellationToken);
        }

        public async Task<GetEventEntry> UpdateEventAsync(Guid eventId, UpsertEventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var user = await _authorizationService.GetUser(userId: @event.UserId);
            var existingEvent = await _eventStore.GetEventAsync(eventId, cancellationToken);

            if (existingEvent is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            existingEvent.PatchFrom(@event);
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new GetEventEntry(result, new GetUserEntry(user), false, false);
        }
    }
}
