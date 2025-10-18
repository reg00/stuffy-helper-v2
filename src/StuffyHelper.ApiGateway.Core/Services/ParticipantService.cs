using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class ParticipantService : IParticipantService
    {
        private readonly IParticipantClient _participantClient;

        public ParticipantService(IParticipantClient participantClient)
        {
            _participantClient = participantClient;
        }

        public async Task<GetParticipantEntry> GetParticipantAsync(string token, Guid eventId, Guid participantId, CancellationToken cancellationToken)
        {
            return await _participantClient.GetParticipantAsync(token, eventId, participantId, cancellationToken);
        }

        public async Task<Response<ParticipantShortEntry>> GetParticipantsAsync(
            string token, 
            Guid eventId,
            int offset = 0,
            int limit = 10,
            string? userId = null,
            CancellationToken cancellationToken = default)
        {
            return await _participantClient.GetParticipantsAsync(token, eventId, offset, limit, userId, cancellationToken);
        }

        public async Task<ParticipantShortEntry> AddParticipantAsync(string token, Guid eventId, UpsertParticipantEntry participant, CancellationToken cancellationToken = default)
        {
            return await _participantClient.CreateParticipantAsync(token, eventId, participant, cancellationToken);
        }

        public async Task DeleteParticipantAsync(string token, Guid eventId, Guid participantId, CancellationToken cancellationToken = default)
        {
            await _participantClient.DeleteParticipantAsync(token, eventId, participantId, cancellationToken);
        }
    }
}
