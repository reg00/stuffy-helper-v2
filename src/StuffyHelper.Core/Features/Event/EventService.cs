using EnsureThat;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using System.Security.Claims;
using AutoMapper;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Minio.Features.Helpers;
using BadRequestException = StuffyHelper.Common.Exceptions.BadRequestException;
using EntityNotFoundException = Reg00.Infrastructure.Errors.EntityNotFoundException;
using ForbiddenException = Reg00.Infrastructure.Errors.ForbiddenException;

namespace StuffyHelper.Core.Features.Event
{
    public class EventService : IEventService
    {
        private readonly IEventStore _eventStore;
        private readonly IParticipantService _participantService;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;
        private readonly IMapper _mapper;

        public EventService(
            IEventStore eventStore,
            IParticipantService participantService,
            IAuthorizationService authorizationService,
            IMediaService mediaService,
            IMapper mapper)
        {
            _eventStore = eventStore;
            _participantService = participantService;
            _authorizationService = authorizationService;
            _mediaService = mediaService;
            _mapper = mapper;
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
                participants.Add(new ParticipantShortEntry(item, _mapper.Map<UserShortEntry>(participantUser)));
            }

            return new GetEventEntry(entry, _mapper.Map<UserShortEntry>(user), participants);
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
                                                        createdDateEnd, eventDateStartMin, eventDateStartMax, eventDateEndMin, eventDateEndMax,
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

            if (eventEntry.EventDateEnd != null && eventEntry.EventDateEnd < eventEntry.EventDateStart)
                throw new BadRequestException("End date must be later than start date.");

            var userName = user.Identity?.Name;

            if (userName == null)
                throw new ForbiddenException("Authorization error");

            var identityUser = await _authorizationService.GetUserByName(userName);
            var entry = new EventEntry(
                eventEntry.Name,
                eventEntry.Description,
                eventEntry.EventDateStart,
                eventEntry.EventDateEnd,
                identityUser.Id);

            var result = await _eventStore.AddEventAsync(entry, cancellationToken);

            var addParticipant = new UpsertParticipantEntry()
            {
                EventId = result.Id,
                UserId = entry.UserId
            };

            await _participantService.AddParticipantAsync(addParticipant, cancellationToken);

            return new EventShortEntry(result);
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
                throw new BadRequestException("Cannot edit completed event");
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
