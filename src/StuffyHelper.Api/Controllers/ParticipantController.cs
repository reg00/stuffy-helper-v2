using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ParticipantController : AuthorizedApiController
    {
        private readonly IParticipantService _participantService;

        private StuffyClaims UserClaims => IdentityClaims.GetUserClaims();
        
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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantsRoute)]
        public async Task<Response<ParticipantShortEntry>> GetAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string? userId = null)
        {
            return await _participantService.GetParticipantsAsync(offset, limit, eventId, userId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение участника ивента по идентификатору
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(GetParticipantEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantRoute)]
        public async Task<GetParticipantEntry> GetAsync(Guid participantId)
        {
            return await _participantService.GetParticipantAsync(participantId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Добавление участника
        /// </summary>
        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(ParticipantShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddParticipantRoute)]
        public async Task<ParticipantShortEntry> PostAsync([FromBody] UpsertParticipantEntry addEntry)
        {
            return await _participantService.AddParticipantAsync(addEntry, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление участника
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteParticipantRoute)]
        public async Task DeleteAsync(Guid participantId)
        {
            await _participantService.DeleteParticipantAsync(UserClaims.UserId, participantId, HttpContext.RequestAborted);
        }

        // <summary>
        // Изменение участника ивента (нужно или нет хз)
        // </summary>
        //[HttpPatch]
        //[Produces(KnownContentTypes.ApplicationJson)]
        //[ProducesResponseType(typeof(ParticipantShortEntry), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        //[Route(KnownRoutes.UpdateParticipantRoute)]
        //public async Task<IActionResult> PatchAsync(Guid participantId, [FromBody] UpsertParticipantEntry updateEntry)
        //{
        //    var entry = await _participantService.UpdateParticipantAsync(participantId, updateEntry, HttpContext.RequestAborted);

        //    return StatusCode((int)HttpStatusCode.OK, entry);
        //}
    }
}
