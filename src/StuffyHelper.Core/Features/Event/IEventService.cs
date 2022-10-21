using StuffyHelper.Core.Features.Common;
using System.Security.Claims;

namespace StuffyHelper.Core.Features.Event
{
    public interface IEventService
    {
        Task<GetEventEntry> GetEventAsync(Guid eventId, CancellationToken cancellationToken);

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
            Guid? shoppingId = null,
            CancellationToken cancellationToken = default);

        Task<EventShortEntry> AddEventAsync(
            AddEventEntry entry,
            ClaimsPrincipal user,
            CancellationToken cancellationToken = default);

        Task DeleteEventAsync(Guid eventId, CancellationToken cancellationToken = default);

        Task<EventShortEntry> UpdateEventAsync(Guid eventId, UpdateEventEntry @event, CancellationToken cancellationToken = default);
    }
}
