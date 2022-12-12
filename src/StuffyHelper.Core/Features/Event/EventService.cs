using EnsureThat;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using System.Security.Claims;

namespace StuffyHelper.Core.Features.Event
{
    public class EventService : IEventService
    {
        private readonly IEventStore _eventStore;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;

        public EventService(
            IEventStore eventStore,
            IAuthorizationService authorizationService,
            IMediaService mediaService)
        {
            _eventStore = eventStore;
            _authorizationService = authorizationService;
            _mediaService = mediaService;
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

        public async Task<EventShortEntry> AddEventAsync(
            AddEventEntry eventEntry,
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(user, nameof(user));
            EnsureArg.IsNotNull(eventEntry, nameof(eventEntry));

            EventEntry result = null;
            MediaShortEntry media = null;
            var identityUser = await _authorizationService.GetUser(userName: user.Identity.Name);
            var entry = new EventEntry(
                eventEntry.Name,
                eventEntry.Description,
                eventEntry.EventDateStart,
                eventEntry.EventDateEnd,
                identityUser.Id);

            if(eventEntry.File != null)
                FileTypeMapper.ValidateExtIsImage(Path.GetExtension(eventEntry.File.FileName));

            try
            {
                result = await _eventStore.AddEventAsync(entry, cancellationToken);

                if (eventEntry.File != null)
                {
                    var addMedia = new AddMediaEntry(result.Id, eventEntry.File, MediaType.Image, null);

                    media = await _mediaService.StoreMediaFormFileAsync(
                            addMedia,
                            isPrimal: true,
                            cancellationToken: cancellationToken);

                    var mediaUri = await _mediaService.GetEventPrimalMediaUri(result.Id);
                    result.ImageUri = mediaUri;
                    result = await _eventStore.UpdateEventAsync(result, cancellationToken);
                }

                return new EventShortEntry(result);
            }
            catch
            {
                if(eventEntry.File != null)
                {
                    await _eventStore.DeleteEventAsync(result.Id, cancellationToken);
                    await _mediaService.DeleteMediaAsync(media.Id, cancellationToken);
                }

                throw;
            }
        }

        public async Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await _eventStore.DeleteEventAsync(eventId, cancellationToken);
        }

        public async Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry @event, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(@event, nameof(@event));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var existingEvent = await _eventStore.GetEventAsync(eventId, cancellationToken);

            if (existingEvent is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            existingEvent.PatchFrom(@event);
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(result);
        }

        public async Task DeletePrimalEventMedia(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var @event = await _eventStore.GetEventAsync(eventId, cancellationToken);

            if (@event is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
                await _mediaService.DeleteMediaAsync(primalMedia.Id);

            @event.ImageUri = null;
            await _eventStore.UpdateEventAsync(@event, cancellationToken);
        }

        public async Task<EventShortEntry> UpdatePrimalEventMediaAsync(Guid eventId, IFormFile file, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNull(file, nameof(file));

            var @event = await _eventStore.GetEventAsync(eventId, cancellationToken);

            if (@event is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
                await _mediaService.DeleteMediaAsync(primalMedia.Id);

            var addMedia = new AddMediaEntry(eventId, file, MediaType.Image, null);
            await _mediaService.StoreMediaFormFileAsync(
                    addMedia,
                    isPrimal: true,
                    cancellationToken: cancellationToken);

            var mediaUri = await _mediaService.GetEventPrimalMediaUri(eventId);
            @event.ImageUri = mediaUri;
            @event = await _eventStore.UpdateEventAsync(@event, cancellationToken);

            return new EventShortEntry(@event);
        }

        public async Task<EventShortEntry> CompleteEventAsync(Guid eventId, ClaimsPrincipal user, bool isComplete, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNull(user, nameof(user));

            var isAdmin = await _authorizationService.CheckUserIsAdmin(user, cancellationToken);
            var stuffyUser = await _authorizationService.GetUser(user.Identity.Name);

            var existingEvent = await _eventStore.GetEventAsync(eventId, cancellationToken);

            if (existingEvent is null)
            {
                throw new ResourceNotFoundException($"Event Id '{eventId}' not found");
            }

            if(!isAdmin || existingEvent.UserId != stuffyUser.Id)
            {
                throw new AuthorizationException($"You don't have permission to complete/uncomplete event.");
            }

            existingEvent.IsCompleted = isComplete;
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(result);
        }
    }
}
