using Microsoft.AspNetCore.Http;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Messages;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    public interface IEventService
    {
        Task<GetEventEntry> GetEventAsync(Guid eventId, string? userId = null, CancellationToken cancellationToken = default);

        Task<Response<EventShortEntry>> GetEventsAsync(
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

        Task<EventShortEntry> AddEventAsync(
            AddEventEntry eventEntry,
            StuffyClaims claims,
            CancellationToken cancellationToken = default);

        Task DeleteEventAsync(Guid eventId, string? userId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry updateEvent, string? userId, CancellationToken cancellationToken = default);

        Task DeletePrimalEventMedia(Guid eventId, string? userId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> UpdatePrimalEventMediaAsync(Guid eventId, IFormFile file, string? userId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> CompleteEventAsync(Guid eventId, string? userId, bool isComplete, CancellationToken cancellationToken = default);
    }
}
