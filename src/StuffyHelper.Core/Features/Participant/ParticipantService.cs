using EnsureThat;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantStore _participantStore;
        private readonly IAuthorizationService _authorizationService;

        public ParticipantService(IParticipantStore participantStore, IAuthorizationService authorizationService)
        {
            _participantStore = participantStore;
            _authorizationService = authorizationService;
        }

        public async Task<GetParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var entry = await _participantStore.GetParticipantAsync(participantId, cancellationToken);
            var user = await _authorizationService.GetUser(userId: entry.UserId);

            return new GetParticipantEntry(entry, new GetUserEntry(user), true, true, true);
        }

        public async Task<Response<GetParticipantEntry>> GetParticipantsAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string userId = null,
            bool? isActive = null,
            CancellationToken cancellationToken = default)
        {
            var resp = await _participantStore.GetParticipantsAsync(offset, limit, eventId, userId, isActive, cancellationToken);
            var participants = new List<GetParticipantEntry>();

            foreach (var @Participant in resp.Data)
            {
                var user = await _authorizationService.GetUser(userId: @Participant.UserId);
                participants.Add(new GetParticipantEntry(@Participant, new GetUserEntry(user), true, true, true));
            }

            return new Response<GetParticipantEntry>()
            {
                Data = participants,
                TotalPages = resp.TotalPages
            };
        }

        public async Task<GetParticipantEntry> AddParticipantAsync(UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));

            var user = await _authorizationService.GetUser(userId: participant.UserId);
            var entry = participant.ToCommonEntry();
            var result = await _participantStore.AddParticipantAsync(entry, cancellationToken);

            return new GetParticipantEntry(result, new GetUserEntry(user), false, false, false);
        }

        public async Task DeleteParticipantAsync(Guid participantId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            await _participantStore.DeleteParticipantAsync(participantId, cancellationToken);
        }

        public async Task<GetParticipantEntry> UpdateParticipantAsync(Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(participant, nameof(participant));
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var user = await _authorizationService.GetUser(userId: participant.UserId);
            var existingParticipant = await _participantStore.GetParticipantAsync(participantId, cancellationToken);

            if (existingParticipant is null)
            {
                throw new ResourceNotFoundException($"Participant Id '{participantId}' not found");
            }

            existingParticipant.PatchFrom(participant);
            var result = await _participantStore.UpdateParticipantAsync(existingParticipant, cancellationToken);

            return new GetParticipantEntry(result, new GetUserEntry(user), false, false, false);
        }
    }
}
