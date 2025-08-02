using Microsoft.AspNetCore.Http;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Clients.Interface;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services
{
    public class EventService : IEventService
    {
        private readonly IEventClient _eventClient;

        public EventService(IEventClient eventClient)
        {
            _eventClient = eventClient;
        }

        public async Task<GetEventEntry> GetEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            return await _eventClient.GetEventAsync(token, eventId, cancellationToken);
        }

        public async Task<Response<EventShortEntry>> GetEventsAsync(
            string token,
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
            return await _eventClient.GetEventsAsync(token, offset, limit, name, description, createdDateStart,
                                                        createdDateEnd, eventDateStartMin, eventDateStartMax, eventDateEndMin, eventDateEndMax,
                                                        userId, isCompleted, isActive, participantId, purchaseId, cancellationToken);
        }

        public async Task<EventShortEntry> AddEventAsync(
            string token,
            AddEventEntry eventEntry,
            CancellationToken cancellationToken = default)
        {
            return await _eventClient.CreateEventAsync(token, eventEntry, cancellationToken);
        }

        public async Task DeleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventClient.DeleteEventAsync(token, eventId, cancellationToken);
        }

        public async Task<EventShortEntry> UpdateEventAsync(string token, Guid eventId, UpdateEventEntry updateEvent, CancellationToken cancellationToken = default)
        {
            return await _eventClient.UpdateEventAsync(token, eventId, updateEvent, cancellationToken);
        }

        public async Task DeletePrimalEventMedia(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventClient.DeleteEventAvatarAsync(token, eventId, cancellationToken);
        }

        public async Task<EventShortEntry> UpdatePrimalEventMediaAsync(string token, Guid eventId, IFormFile file, CancellationToken cancellationToken = default)
        {
            return await _eventClient.UpdateEventAvatarAsync(token, eventId, file, cancellationToken);
        }

        public async Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventClient.CompleteEventAsync(token, eventId, cancellationToken);
        }
        
        public async Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventClient.ReopenEventAsync(token, eventId, cancellationToken);
        }

        public async Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default)
        {
            await _eventClient.CheckoutEventAsync(token, eventId, cancellationToken);
        }
    }
}
