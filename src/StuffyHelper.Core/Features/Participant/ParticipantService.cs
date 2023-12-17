using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantStore _participantStore;
        private readonly IAuthorizationService _authorizationService;

        public ParticipantService(
            IParticipantStore participantStore,
            IAuthorizationService authorizationService)
        {
            _participantStore = participantStore;
            _authorizationService = authorizationService;
        }

        public async Task<GetParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var entry = await _participantStore.GetParticipantAsync(participantId, cancellationToken);
            var user = await _authorizationService.GetUserById(entry.UserId);

            return new GetParticipantEntry(entry, new GetUserEntry(user), entry.PurchaseUsages.Select(x => new PurchaseUsageShortEntry(x)));
        }

        public async Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _participantStore.GetParticipantsAsync(offset, limit, eventId, userId, cancellationToken);
            var participants = new List<ParticipantShortEntry>();

            foreach (var participant in resp.Data)
            {
                var user = await _authorizationService.GetUserById(participant.UserId);
                participants.Add(new ParticipantShortEntry(participant, new UserShortEntry(user)));
            }

            return new Response<ParticipantShortEntry>()
            {
                Data = participants,
                TotalPages = resp.TotalPages,
                Total = resp.Total
            };
        }

        public async Task<ParticipantShortEntry> AddParticipantAsync(UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            var user = await _authorizationService.GetUserById(participant.UserId);
            var entry = participant.ToCommonEntry();
            var result = await _participantStore.AddParticipantAsync(entry, cancellationToken);

            return new ParticipantShortEntry(result, new UserShortEntry(user));
        }

        public async Task DeleteParticipantAsync(string userId, Guid participantId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var participant = await _participantStore.GetParticipantAsync(participantId, cancellationToken);

            if (participant is null)
                throw new EntityNotFoundException($"Participant with Id '{participantId}' not found.");

            if (userId != participant.Event.UserId && participant.UserId != userId)
                throw new ForbiddenException("Cannot delete participant if you are not an owner of event");

            var owner = participant.Event.Participants.FirstOrDefault(x => x.UserId == userId);
            if (participantId == owner!.Id)
                throw new BadRequestException("Cannot remove yourself");

            if (participant.Event.Participants.Count == 1)
                throw new BadRequestException("Cannot remove last participant from event");

            await _participantStore.DeleteParticipantAsync(participant, cancellationToken);
        }

        public async Task<ParticipantShortEntry> UpdateParticipantAsync(Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var existingParticipant = await _participantStore.GetParticipantAsync(participantId, cancellationToken);

            if (existingParticipant is null)
            {
                throw new EntityNotFoundException($"Participant Id '{participantId}' not found");
            }

            var user = await _authorizationService.GetUserById(participant.UserId);

            existingParticipant.PatchFrom(participant);
            var result = await _participantStore.UpdateParticipantAsync(existingParticipant, cancellationToken);

            return new ParticipantShortEntry(result, new UserShortEntry(user));
        }
    }
}
