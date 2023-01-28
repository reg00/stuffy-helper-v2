using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Event;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    [Authorize]
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <summary>
        /// Получение списка ивентов
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<EventShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetEventsRoute)]
        public async Task<IActionResult> GetAsync(
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
            Guid? purchaseId = null)
        {
            var eventResponse = await _eventService.GetEventsAsync(offset, limit, name, description, createdDateStart, createdDateEnd,
                                                                   eventDateStartMin, eventDateStartMax, eventDateEndMin, eventDateEndMax, userId,
                                                                   isCompleted, isActive, participantId, purchaseId, HttpContext.RequestAborted);

            return Ok(eventResponse);
        }

        /// <summary>
        /// Получение ивента по его идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetEventEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetEventRoute)]
        public async Task<IActionResult> GetAsync(Guid eventId)
        {
            var @event = await _eventService.GetEventAsync(eventId, HttpContext.RequestAborted);

            return Ok(@event);
        }

        /// <summary>
        /// Добавление ивента
        /// </summary>
        [HttpPost]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddEventRoute)]
        public async Task<IActionResult> PostAsync(
             [FromForm] AddEventEntry entry)
        {
            var @event = await _eventService.AddEventAsync(
                entry,
                User,
                HttpContext.RequestAborted);

            return Ok(@event);
        }

        /// <summary>
        /// Удаление ивента
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteEventRoute)]
        public async Task<IActionResult> DeleteAsync(Guid eventId)
        {
            await _eventService.DeleteEventAsync(eventId, User.Identity?.Name, HttpContext.RequestAborted);

            return Ok();
        }

        /// <summary>
        /// Извенение данных по ивенту
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateEventRoute)]
        public async Task<IActionResult> PatchAsync(Guid eventId, [FromBody] UpdateEventEntry updateEntry)
        {
            var entry = await _eventService.UpdateEventAsync(eventId, updateEntry, User.Identity?.Name, HttpContext.RequestAborted);

            return Ok(entry);
        }

        /// <summary>
        /// Удаление обложки ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteEventPrimalMediaRoute)]
        public async Task<IActionResult> DeletePrimalMediaAsync([FromRoute] Guid eventId)
        {
            await _eventService.DeletePrimalEventMedia(eventId, User.Identity?.Name, HttpContext.RequestAborted);

            return Ok();
        }

        /// <summary>
        /// Обновление обложки ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateEventPrimalMediaRoute)]
        public async Task<IActionResult> PatchPrimalMediaAsync([FromRoute] Guid eventId, IFormFile file)
        {
            var @event = await _eventService.UpdatePrimalEventMediaAsync(eventId, file, User.Identity?.Name, HttpContext.RequestAborted);

            return Ok(@event);
        }

        /// <summary>
        /// Завершение ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.CompleteEventRoute)]
        public async Task<IActionResult> CompleteEventAsync([FromRoute] Guid eventId)
        {
            await _eventService.CompleteEventAsync(eventId, User.Identity?.Name, true, HttpContext.RequestAborted);

            return Ok();
        }

        /// <summary>
        /// Завершение ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.ReopenEventRoute)]
        public async Task<IActionResult> ReopenEventAsync([FromRoute] Guid eventId)
        {
            await _eventService.CompleteEventAsync(eventId, User.Identity?.Name, false, HttpContext.RequestAborted);

            return Ok();
        }
    }
}
