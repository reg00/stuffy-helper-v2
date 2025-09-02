using AutoMapper;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Clients.Interface;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;

namespace StuffyHelper.Core.Services
{
    /// <inheritdoc />
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantRepository _participantRepository;
        private readonly IAuthorizationClient _authorizationClient;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public ParticipantService(
            IParticipantRepository participantRepository,
            IAuthorizationClient authorizationClient,
            IMapper mapper)
        {
            _participantRepository = participantRepository;
            _authorizationClient = authorizationClient;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<GetParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var entry = await _participantRepository.GetParticipantAsync(participantId, cancellationToken);
            var user = await _authorizationClient.GetUserById(entry.UserId, cancellationToken);

            //return new GetParticipantEntry(entry, _mapper.Map<GetUserEntry>(user), entry.PurchaseUsages.Select(x => new PurchaseUsageShortEntry(x)));

            return _mapper.Map<GetParticipantEntry>((entry, _mapper.Map<GetUserEntry>(user)));
        }

        /// <inheritdoc />
        public async Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _participantRepository.GetParticipantsAsync(offset, limit, eventId, userId, cancellationToken);
            var participants = new List<ParticipantShortEntry>();

            foreach (var participant in resp.Data)
            {
                var user = await _authorizationClient.GetUserById(participant.UserId, cancellationToken);
                participants.Add(_mapper.Map<ParticipantShortEntry>((participant, _mapper.Map<UserShortEntry>(user))));
            }

            return new Response<ParticipantShortEntry>()
            {
                Data = participants,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        /// <inheritdoc />
        public async Task<ParticipantShortEntry> AddParticipantAsync(UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            var user = await _authorizationClient.GetUserById(participant.UserId, cancellationToken);
            var entry = _mapper.Map<ParticipantEntry>(participant);
            var result = await _participantRepository.AddParticipantAsync(entry, cancellationToken);

            return _mapper.Map<ParticipantShortEntry>((result, _mapper.Map<UserShortEntry>(user)));
        }

        /// <inheritdoc />
        public async Task DeleteParticipantAsync(string userId, Guid participantId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var participant = await _participantRepository.GetParticipantAsync(participantId, cancellationToken);

            if (participant is null)
                throw new EntityNotFoundException("Participant {ParticipantId} not found.", participantId);

            // Нельзя удалить если ивент завершен
            if (participant.Event.IsCompleted)
                throw new BadRequestException("Cannot delete participant {ParticipantId} from completed event {EventId}. User: {UserId}", participant.Id, participant.EventId, userId);

            // Нельзя удалить владельца ивента
            if (participant.UserId == participant.Event.UserId)
                throw new ForbiddenException("Cannot remove event owner {UserId}", participant.Event.UserId);

            // Нельзя удалить участника (кроме себя), если ты не владелец ивента
            if (userId != participant.Event.UserId && participant.UserId != userId)
                throw new ForbiddenException("Cannot delete participant {ParticipantId} if you are not an owner of event. User: {UserId}", participant.Id, userId);
            
            // Нельзя удалить участника, если у него есть рассчитанные покупки
            if(participant.Purchases.Any(x => x.IsComplete))
                throw new BadRequestException("Cannot remove participant {ParticipantId} with completed purchases. User: {UserId}", participant.Id, userId);

            // Нельзя удалить участника, если у него есть долги
            if(participant.Event.Debts.Any(x => x.DebtorId == participant.UserId || x.LenderId == participant.UserId))
                throw new BadRequestException("Cannot remove participant {ParticipantId} with debts. User: {UserId}", participant.Id, userId);

            await _participantRepository.DeleteParticipantAsync(participant, cancellationToken);
        }

        /// <inheritdoc />
        public async Task<ParticipantShortEntry> UpdateParticipantAsync(Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var existingParticipant = await _participantRepository.GetParticipantAsync(participantId, cancellationToken);

            if (existingParticipant is null)
            {
                throw new EntityNotFoundException("Participant {ParticipantId} not found.", participantId);
            }

            var user = await _authorizationClient.GetUserById(participant.UserId, cancellationToken);

            existingParticipant.PatchFrom(participant);
            var result = await _participantRepository.UpdateParticipantAsync(existingParticipant, cancellationToken);

            return _mapper.Map<ParticipantShortEntry>((result, _mapper.Map<UserShortEntry>(user)));
        }
    }
}
