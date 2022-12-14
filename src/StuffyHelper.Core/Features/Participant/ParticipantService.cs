using EnsureThat;
using StuffyHelper.Authorization.Core.Features;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.PurchaseUsage;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantStore _participantStore;
        private readonly IAuthorizationService _authorizationService;
        private readonly IMediaService _mediaService;

        public ParticipantService(
            IParticipantStore participantStore,
            IAuthorizationService authorizationService,
            IMediaService mediaService)
        {
            _participantStore = participantStore;
            _authorizationService = authorizationService;
            _mediaService = mediaService;
        }

        public async Task<GetParticipantEntry> GetParticipantAsync(Guid participantId, CancellationToken cancellationToken)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            var entry = await _participantStore.GetParticipantAsync(participantId, cancellationToken);
            var user = await _authorizationService.GetUser(userId: entry.UserId);

            var purchaseUsages = new List<PurchaseUsageShortEntry>();

            foreach (var item in entry.PurchaseUsages)
            {
                var purchaseUsageUser = await _authorizationService.GetUser(userId: item.Participant.UserId);
                purchaseUsages.Add(new PurchaseUsageShortEntry(item, purchaseUsageUser));
            }

            return new GetParticipantEntry(entry, new UserShortEntry(user), purchaseUsages);
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

            foreach (var @Participant in resp.Data)
            {
                var user = await _authorizationService.GetUser(userId: @Participant.UserId);
                participants.Add(new ParticipantShortEntry(@Participant, new UserShortEntry(user)));
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

            var user = await _authorizationService.GetUser(userId: participant.UserId);
            var entry = participant.ToCommonEntry();
            var result = await _participantStore.AddParticipantAsync(entry, cancellationToken);

            return new ParticipantShortEntry(result, new UserShortEntry(user));
        }

        public async Task DeleteParticipantAsync(Guid participantId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(participantId, nameof(participantId));

            await _participantStore.DeleteParticipantAsync(participantId, cancellationToken);
        }

        public async Task<ParticipantShortEntry> UpdateParticipantAsync(Guid participantId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
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

            return new ParticipantShortEntry(result, new UserShortEntry(user));
        }
    }
}
