using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Event;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetEventEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetEventsRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            string name = null,
            string description = null,
            DateTime? createdDateStart = null,
            DateTime? createdDateEnd = null,
            DateTime? eventDateStart = null,
            DateTime? eventDateEnd = null,
            string userId = null,
            bool? isCompleted = null,
            bool? isActive = null,
            Guid? participantId = null,
            Guid? shoppingId = null)
        {
            var eventResponse = await _eventService.GetEventsAsync(offset, limit, name, description, createdDateStart, createdDateEnd,
                                                                   eventDateStart, eventDateEnd, userId, isCompleted, isActive,
                                                                   participantId, shoppingId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, eventResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetEventEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetEventRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid eventId)
        {
            var eventEntry = await _eventService.GetEventAsync(eventId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, eventEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetEventEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddEventRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertEventEntry addEntry)
        {
            var @event = await _eventService.AddEventAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, @event);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteEventRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid eventId)
        {
            await _eventService.DeleteEventAsync(eventId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetEventEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateEventRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid eventId, [FromBody] UpsertEventEntry updateEntry)
        {
            var entry = await _eventService.UpdateEventAsync(eventId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
