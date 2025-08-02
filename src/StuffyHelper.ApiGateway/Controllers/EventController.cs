using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    [Authorize]
    public class EventController : AuthorizedApiController
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
        public async Task<Response<EventShortEntry>> GetAsync(
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
            return await _eventService.GetEventsAsync(Token, offset, limit, name, description, createdDateStart, createdDateEnd,
                                                                   eventDateStartMin, eventDateStartMax, eventDateEndMin, eventDateEndMax, userId,
                                                                   isCompleted, isActive, participantId, purchaseId, HttpContext.RequestAborted);
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
        public async Task<GetEventEntry> GetAsync(Guid eventId)
        {
            return await _eventService.GetEventAsync(Token, eventId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление ивента
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddEventRoute)]
        public async Task<EventShortEntry> PostAsync([FromBody] AddEventEntry entry)
        {
            return await _eventService.AddEventAsync(Token, entry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление ивента
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteEventRoute)]
        public async Task DeleteAsync(Guid eventId)
        {
            await _eventService.DeleteEventAsync(Token, eventId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Извенение данных по ивенту
        /// </summary>
        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(EventShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateEventRoute)]
        public async Task<EventShortEntry> PatchAsync(Guid eventId, [FromBody] UpdateEventEntry updateEntry)
        {
            return await _eventService.UpdateEventAsync(Token, eventId, updateEntry, HttpContext.RequestAborted);
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
        public async Task DeletePrimalMediaAsync([FromRoute] Guid eventId)
        {
            await _eventService.DeletePrimalEventMedia(Token, eventId, HttpContext.RequestAborted);
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
        public async Task<EventShortEntry> PatchPrimalMediaAsync([FromRoute] Guid eventId, IFormFile file)
        {
            return await _eventService.UpdatePrimalEventMediaAsync(Token,eventId, file, HttpContext.RequestAborted);
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
        public async Task CompleteEventAsync([FromRoute] Guid eventId)
        {
            await _eventService.CompleteEventAsync(Token, eventId, HttpContext.RequestAborted);
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
        public async Task ReopenEventAsync([FromRoute] Guid eventId)
        {
            await _eventService.ReopenEventAsync(Token, eventId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Расчет долгов по ивенту
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.CheckoutEventRoute)]
        public async Task CheckoutEventAsync([FromRoute] Guid eventId)
        {
            await _eventService.CheckoutEventAsync(Token, eventId, HttpContext.RequestAborted);
        }
    }
}
