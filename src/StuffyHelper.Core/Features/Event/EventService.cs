﻿using EnsureThat;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Participant;
using System.Security.Claims;

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

            var participants = new List<ParticipantShortEntry>();

            foreach (var item in entry.Participants)
            {
                var participantUser = await _authorizationService.GetUser(userId: item.UserId);
                participants.Add(new ParticipantShortEntry(item, new UserShortEntry(participantUser)));
            }

            return new GetEventEntry(entry, new UserShortEntry(user), participants);
        }

        public async Task<Response<EventShortEntry>> GetEventsAsync(
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
            var events = new List<EventShortEntry>();

            foreach (var @event in resp.Data)
                events.Add(new EventShortEntry(@event));

            return new Response<EventShortEntry>()
            {
                Data = events,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<EventShortEntry> AddEventAsync(UpsertEventEntry @event, ClaimsPrincipal user, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));
            EnsureArg.IsNotNull(user, nameof(user));

            var identityUser = await _authorizationService.GetUser(userName: user.Identity.Name);
            var entry = @event.ToCommonEntry(identityUser.Id);
            var result = await _eventStore.AddEventAsync(entry, cancellationToken);

            return new EventShortEntry(result);
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await _eventStore.DeleteEventAsync(eventId, cancellationToken);
        }

        public async Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpsertEventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var existingEvent = await _eventStore.GetEventAsync(eventId, cancellationToken);
            var user = await _authorizationService.GetUser(userId: existingEvent.UserId);

            if (existingEvent is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            existingEvent.PatchFrom(@event);
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(result);
        }
    }
}
