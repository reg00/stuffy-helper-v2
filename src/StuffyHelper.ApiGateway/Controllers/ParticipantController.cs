using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class ParticipantController : AuthorizedApiController
    {
        private readonly IParticipantService _participantService;
        
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
            return await _participantService.GetParticipantsAsync(Token, eventId, offset, limit, userId, HttpContext.RequestAborted);
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
            return await _participantService.GetParticipantAsync(Token, eventId, participantId, HttpContext.RequestAborted);
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
            return await _participantService.AddParticipantAsync(Token, eventId, addEntry, HttpContext.RequestAborted);
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
            await _participantService.DeleteParticipantAsync(Token, eventId, participantId, HttpContext.RequestAborted);
        }
    }
}
