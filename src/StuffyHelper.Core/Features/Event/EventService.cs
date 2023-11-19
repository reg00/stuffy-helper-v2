using EnsureThat;
using Microsoft.AspNetCore.Http;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Minio.Features.Common;
using System.Security.Claims;

namespace StuffyHelper.Core.Features.Event
{
    public class EventService : IEventService
    {
        private readonly IEventStore _eventStore;
        private readonly IParticipantService _participantService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;

        public EventService(
            IEventStore eventStore,
            IParticipantService participantService,
            IAuthorizationService authorizationService,
            IMediaService mediaService)
        {
            _eventStore = eventStore;
            _participantService = participantService;
            _authorizationService = authorizationService;
            _mediaService = mediaService;
        }

        public async Task<GetEventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var entry = await _eventStore.GetEventAsync(eventId, userId, cancellationToken);
            var user = await _authorizationService.GetUserById(entry.UserId);

            var participants = new List<ParticipantShortEntry>();

            foreach (var item in entry.Participants)
            {
                var participantUser = await _authorizationService.GetUserById(item.UserId);
                participants.Add(new ParticipantShortEntry(item, new UserShortEntry(participantUser)));
            }

            return new GetEventEntry(entry, new UserShortEntry(user), participants);
        }

        public async Task<Response<EventShortEntry>> GetEventsAsync(
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
            var resp = await _eventStore.GetEventsAsync(offset, limit, name, description, createdDateStart,
                                                        createdDateEnd, eventDateStartMin, eventDateStartMax, eventDateEndMin , eventDateEndMax,
                                                        userId, isCompleted, isActive, participantId, purchaseId, cancellationToken);
            
            return new Response<EventShortEntry>()
            {
                Data = resp.Data.Select(x => new EventShortEntry(x)),
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

            EventEntry? result = null;
            MediaShortEntry? media = null;
            ParticipantShortEntry? participant = null;

            var userName = user?.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var identityUser = await _authorizationService.GetUserByName(userName);
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
                    var addMedia = new AddMediaEntry(result.Id, eventEntry.File, MediaType.Image, string.Empty);

                    media = await _mediaService.StoreMediaFormFileAsync(
                            addMedia,
                            isPrimal: true,
                            cancellationToken: cancellationToken);

                    var mediaUri = await _mediaService.GetEventPrimalMediaUri(result.Id);
                    result.ImageUri = mediaUri;
                    result = await _eventStore.UpdateEventAsync(result, cancellationToken);
                }

                var addParticipant = new UpsertParticipantEntry()
                {
                    EventId = result.Id,
                    UserId = entry.UserId
                };

                participant = await _participantService.AddParticipantAsync(addParticipant, cancellationToken);

                return new EventShortEntry(result);
            }
            catch
            {
                if(eventEntry.File != null)
                {
                    if(result != null)
                        await _eventStore.DeleteEventAsync(result.Id, cancellationToken);
                    
                    if(media != null)
                        await _mediaService.DeleteMediaAsync(media.Id, cancellationToken);

                    if(participant != null)
                        await _participantService.DeleteParticipantAsync(participant.Id, cancellationToken);
                }

                throw;
            }
        }

        public async Task DeleteEventAsync(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await ValidateEventAsync(eventId, userId, true, cancellationToken);

            await _eventStore.DeleteEventAsync(eventId, cancellationToken);
        }

        public async Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry updateEvent, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(updateEvent, nameof(updateEvent));

            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);

            existingEvent.PatchFrom(updateEvent);
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(result);
        }

        public async Task DeletePrimalEventMedia(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);

            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
                await _mediaService.DeleteMediaAsync(primalMedia.Id);

            existingEvent.ImageUri = null;
            await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);
        }

        public async Task<EventShortEntry> UpdatePrimalEventMediaAsync(Guid eventId, IFormFile file, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(file, nameof(file));

            FileTypeMapper.ValidateExtIsImage(Path.GetExtension(file.FileName));

            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);
            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
                await _mediaService.DeleteMediaAsync(primalMedia.Id);

            var addMedia = new AddMediaEntry(eventId, file, MediaType.Image, string.Empty);
            await _mediaService.StoreMediaFormFileAsync(
                    addMedia,
                    isPrimal: true,
                    cancellationToken: cancellationToken);

            var mediaUri = await _mediaService.GetEventPrimalMediaUri(eventId);
            existingEvent.ImageUri = mediaUri;
            existingEvent = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(existingEvent);
        }

        public async Task<EventShortEntry> CompleteEventAsync(Guid eventId, string? userId, bool isComplete, CancellationToken cancellationToken = default)
        {
            var existingEvent = await ValidateEventAsync(eventId, userId, false, cancellationToken);

            existingEvent.IsCompleted = isComplete;
            var result = await _eventStore.UpdateEventAsync(existingEvent, cancellationToken);

            return new EventShortEntry(result);
        }

        private async Task<EventEntry> ValidateEventAsync(
            Guid eventId,
            string? userId,
            bool checkIsComplete = true,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var existingEvent = await _eventStore.GetEventAsync(eventId, userId, cancellationToken);

            if (existingEvent is null)
            {
                throw new EntityNotFoundException($"Event Id '{eventId}' not found");
            }

            if (checkIsComplete && existingEvent.IsCompleted)
            {
                throw new StuffyException("Cannot edit completed event");
            }

            await CheckEventPermissionsAsync(existingEvent.UserId, userId);

            return existingEvent;
        }

        private async Task CheckEventPermissionsAsync(string eventUserId, string? userId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(eventUserId, nameof(eventUserId));

            if (string.IsNullOrWhiteSpace(userId))
                return;

            var stuffyUser = await _authorizationService.GetUserById(userId);

            if (eventUserId != stuffyUser.Id)
            {
                throw new AuthorizationException($"You don't have permission to complete/uncomplete event.");
            }
        }
    }
}
