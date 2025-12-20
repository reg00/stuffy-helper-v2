using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Contracts;
using StuffyHelper.Common.Helpers;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Участники ивента
    /// </summary>
    public class ParticipantController : AuthorizedApiController
    {
        private readonly IParticipantService _participantService;

        private StuffyClaims UserClaims => IdentityClaims.GetUserClaims();
        
        /// <summary>
        /// Ctor
        /// </summary>
        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        /// <summary>
        /// Получение списка участников ивента
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(Response<ParticipantShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantsRoute)]
        public async Task<Response<ParticipantShortEntry>> GetAsync(
            [FromRoute] Guid eventId,
            int offset = 0,
            int limit = 10,
            string? userId = null)
        {
            return await _participantService.GetParticipantsAsync(eventId, offset, limit, userId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение участника ивента по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetParticipantEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantRoute)]
        public async Task<GetParticipantEntry> GetAsync([FromRoute] Guid eventId, [FromRoute] Guid participantId)
        {
            return await _participantService.GetParticipantAsync(eventId, participantId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление участника
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(ParticipantShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddParticipantRoute)]
        public async Task<ParticipantShortEntry> PostAsync([FromRoute] Guid eventId, [FromBody] UpsertParticipantEntry addEntry)
        {
            return await _participantService.AddParticipantAsync(eventId, addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление участника
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteParticipantRoute)]
        public async Task DeleteAsync([FromRoute] Guid eventId, [FromRoute] Guid participantId)
        {
            await _participantService.DeleteParticipantAsync(UserClaims.UserId, eventId, participantId, HttpContext.RequestAborted);
        }

        // <summary>
        // Изменение участника ивента (нужно или нет хз)
        // </summary>
        //[HttpPatch]
        //[Produces(KnownContentTypes.ApplicationJson)]
        //[ProducesResponseType(typeof(ParticipantShortEntry), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        //[Route(KnownRoutes.UpdateParticipantRoute)]
        //public async Task<IActionResult> PatchAsync(Guid participantId, [FromBody] UpsertParticipantEntry updateEntry)
        //{
        //    var entry = await _participantService.UpdateParticipantAsync(participantId, updateEntry, HttpContext.RequestAborted);

        //    return StatusCode((int)HttpStatusCode.OK, entry);
        //}
    }
}
