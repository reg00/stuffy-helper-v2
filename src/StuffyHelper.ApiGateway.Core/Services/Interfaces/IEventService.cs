using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Core.Services.Interfaces
{
    public interface IEventService
    {
        Task<GetEventEntry> GetEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

        Task<Response<EventShortEntry>> GetEventsAsync(
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
            CancellationToken cancellationToken = default);

        Task<EventShortEntry> AddEventAsync(string token, AddEventEntry eventEntry, CancellationToken cancellationToken = default);

        Task DeleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> UpdateEventAsync(string token, Guid eventId, UpdateEventEntry updateEvent, CancellationToken cancellationToken = default);

       Task DeletePrimalEventMedia(string token, Guid eventId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> UpdatePrimalEventMediaAsync(string token, Guid eventId, IFormFile file, CancellationToken cancellationToken = default);

        Task CompleteEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);

        Task ReopenEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);
        
        Task CheckoutEventAsync(string token, Guid eventId, CancellationToken cancellationToken = default);
    }
}
