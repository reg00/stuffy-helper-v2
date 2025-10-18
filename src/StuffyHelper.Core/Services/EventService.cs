using AutoMapper;
using EnsureThat;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Core.Services
{
    /// <inheritdoc />
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;
        private readonly IParticipantService _participantService;
        private readonly IMediaService _mediaService;
        private readonly IAuthorizationClient _authorizationClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public EventService(
            IEventRepository eventRepository,
            IParticipantService participantService,
            IMediaService mediaService,
            IAuthorizationClient authorizationClient,
            IMapper mapper)
        {
            _eventRepository = eventRepository;
            _participantService = participantService;
            _mediaService = mediaService;
            _authorizationClient = authorizationClient;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetEventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var entry = await _eventRepository.GetEventAsync(eventId, userId, cancellationToken);

            var participants = new List<ParticipantShortEntry>();

            foreach (var item in entry.Participants)
            {
                var participantUser = await _authorizationClient.GetUserById(item.UserId, cancellationToken);
                participants.Add(_mapper.Map<ParticipantShortEntry>((item, _mapper.Map<UserShortEntry>(participantUser))));
            }

            var user = await _authorizationClient.GetUserById(entry.UserId, cancellationToken);
            
            return _mapper.Map<GetEventEntry>((entry, _mapper.Map<UserShortEntry>(user), participants));
        }

        /// <inheritdoc />
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
            var resp = await _eventRepository.GetEventsAsync(offset, limit, name, description, createdDateStart,
                                                        createdDateEnd, eventDateStartMin, eventDateStartMax, eventDateEndMin, eventDateEndMax,
                                                        userId, isCompleted, isActive, participantId, purchaseId, cancellationToken);

            return new Response<EventShortEntry>()
            {
                Data = resp.Data.Select(x => _mapper.Map<EventShortEntry>(x)),
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<EventShortEntry> AddEventAsync(
            AddEventEntry eventEntry,
            StuffyClaims claims,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(claims, nameof(claims));
            EnsureArg.IsNotNull(eventEntry, nameof(eventEntry));

            if (eventEntry.EventDateEnd != null && eventEntry.EventDateEnd < eventEntry.EventDateStart)
                throw new BadRequestException("End date {EndDate} must be later than start date {StartDate}.", eventEntry.EventDateEnd, eventEntry.EventDateStart);

            var identityUser = await _authorizationClient.GetUserById(claims.UserId, cancellationToken);
            var entry = _mapper.Map<EventEntry>((eventEntry, identityUser));
            
            var result = await _eventRepository.AddEventAsync(entry, cancellationToken);

            var addParticipant = new UpsertParticipantEntry()
            {
                UserId = entry.UserId
            };

            await _participantService.AddParticipantAsync(result.Id, addParticipant, cancellationToken);

            return _mapper.Map<EventShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeleteEventAsync(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            await ValidateEventAsync(eventId, userId, true, cancellationToken);

            await _eventRepository.DeleteEventAsync(eventId, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry updateEvent, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(updateEvent, nameof(updateEvent));

            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);

            existingEvent.PatchFrom(updateEvent);
            var result = await _eventRepository.UpdateEventAsync(existingEvent, cancellationToken);

            return _mapper.Map<EventShortEntry>(result);
        }

        /// <inheritdoc />
        public async Task DeletePrimalEventMedia(Guid eventId, string? userId, CancellationToken cancellationToken = default)
        {
            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);

            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
            {
                await _mediaService.DeleteMediaAsync(eventId, primalMedia.Id, cancellationToken);
                existingEvent.ImageUri = null;
                await _eventRepository.UpdateEventAsync(existingEvent, cancellationToken);
            }
        }

        /// <inheritdoc />
        public async Task<EventShortEntry> UpdatePrimalEventMediaAsync(Guid eventId, IFormFile file, string? userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(file, nameof(file));

            FileTypeMapper.ValidateExtIsImage(Path.GetExtension(file.FileName));

            var existingEvent = await ValidateEventAsync(eventId, userId, true, cancellationToken);
            var primalMedia = await _mediaService.GetPrimalEventMedia(eventId, cancellationToken);

            if (primalMedia is not null)
                await _mediaService.DeleteMediaAsync(eventId, primalMedia.Id, cancellationToken);

            var addMedia = _mapper.Map<AddMediaEntry>((eventId, file));
            await _mediaService.StoreMediaFormFileAsync(
                eventId,    
                addMedia,
                isPrimal: true,
                cancellationToken: cancellationToken);

            var mediaUri = await _mediaService.GetEventPrimalMediaUri(eventId, cancellationToken);

            if (mediaUri == null)
                throw new BadRequestException("Error while uploading image {ImageName}. Event: {EventId}", file.FileName, eventId);
            
            existingEvent.ImageUri = mediaUri;
            existingEvent = await _eventRepository.UpdateEventAsync(existingEvent, cancellationToken);

            return _mapper.Map<EventShortEntry>(existingEvent);
        }

        /// <inheritdoc />
        public async Task<EventShortEntry> CompleteEventAsync(Guid eventId, string? userId, bool isComplete, CancellationToken cancellationToken = default)
        {
            var existingEvent = await ValidateEventAsync(eventId, userId, false, cancellationToken);

            existingEvent.IsCompleted = isComplete;
            var result = await _eventRepository.UpdateEventAsync(existingEvent, cancellationToken);

            return _mapper.Map<EventShortEntry>(result);
        }

        /// <summary>
        /// Validate event
        /// </summary>
        private async Task<EventEntry> ValidateEventAsync(
            Guid eventId,
            string? userId,
            bool checkIsComplete = true,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var existingEvent = await _eventRepository.GetEventAsync(eventId, userId, cancellationToken);

            if (existingEvent is null)
            {
                throw new EntityNotFoundException("Event {EventId} not found.", eventId);
            }

            if (checkIsComplete && existingEvent.IsCompleted)
            {
                throw new BadRequestException("Cannot edit completed event {EventId}. User: {UserId}", eventId, userId);
            }

            CheckEventPermissionsAsync(eventId, existingEvent.UserId, userId);

            return existingEvent;
        }

        /// <summary>
        /// Check event permissions
        /// </summary>
        private static void CheckEventPermissionsAsync(Guid eventId, string eventUserId, string? userId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(eventUserId, nameof(eventUserId));

            if (string.IsNullOrWhiteSpace(userId))
                return;

            if (eventUserId != userId)
            {
                throw new ForbiddenException("You don't have permission to complete/uncomplete event. {EventId}", eventId);
            }
        }
    }
}
