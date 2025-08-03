using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Helpers;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Helpers;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Ивенты
    /// </summary>
    [Authorize]
    public class EventController : AuthorizedApiController
    {
        private readonly IEventService _eventService;
        private readonly IDebtService _debtService;

        private StuffyClaims UserClaims => IdentityClaims.GetUserClaims();
        
        public EventController(
            IEventService eventService,
            IDebtService debtService)
        {
            _eventService = eventService;
            _debtService = debtService;
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
            userId = PermissionHelper.GetUserId(UserClaims, userId);

            return await _eventService.GetEventsAsync(offset, limit, name, description, createdDateStart, createdDateEnd,
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
            var userId = PermissionHelper.GetUserId(UserClaims);
            return await _eventService.GetEventAsync(UserClaims, eventId, userId, HttpContext.RequestAborted);
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
            return await _eventService.AddEventAsync(
                entry,
                UserClaims,
                HttpContext.RequestAborted);
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
            await _eventService.DeleteEventAsync(eventId, UserClaims.UserId, HttpContext.RequestAborted);
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
            var userId = PermissionHelper.GetUserId(UserClaims);

            return await _eventService.UpdateEventAsync(eventId, updateEntry, userId, HttpContext.RequestAborted);
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
            var userId = PermissionHelper.GetUserId(UserClaims);

            await _eventService.DeletePrimalEventMedia(eventId, userId, HttpContext.RequestAborted);
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
            var userId = PermissionHelper.GetUserId(UserClaims);

            return await _eventService.UpdatePrimalEventMediaAsync(eventId, file, userId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Завершение ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.CompleteEventRoute)]
        public async Task CompleteEventAsync([FromRoute] Guid eventId)
        {
            var userId = PermissionHelper.GetUserId(UserClaims);

            await _eventService.CompleteEventAsync(eventId, userId, true, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Завершение ивента
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.ReopenEventRoute)]
        public async Task ReopenEventAsync([FromRoute] Guid eventId)
        {
            var userId = PermissionHelper.GetUserId(UserClaims);

            await _eventService.CompleteEventAsync(eventId, userId, false, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Расчет долгов по ивенту
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.CheckoutEventRoute)]
        public async Task CheckoutEventAsync([FromRoute] Guid eventId)
        {
            var userId = PermissionHelper.GetUserId(UserClaims);

            await _debtService.CheckoutEvent(eventId, userId, HttpContext.RequestAborted);
        }
    }
}
