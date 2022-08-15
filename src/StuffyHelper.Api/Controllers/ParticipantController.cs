using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Participant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StuffyHelper.Api.Controllers
{
    internal class ParticipantController : Controller
    {
        private readonly IParticipantService _participantService;

        public ParticipantController(IParticipantService participantService)
        {
            _participantService = participantService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(Response<GetParticipantEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantsRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            string userId = null,
            bool? isActive = null)
        {
            var participantResponse = await _participantService.GetParticipantsAsync(offset, limit, eventId, userId, isActive, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, participantResponse);
        }

        [HttpGet]
        [ProducesResponseType(typeof(GetParticipantEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetParticipantRoute)]
        [Authorize]
        public async Task<IActionResult> GetAsync(Guid participantId)
        {
            var participantEntry = await _participantService.GetParticipantAsync(participantId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, participantEntry);
        }

        [HttpPost]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetParticipantEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.AddParticipantRoute)]
        [Authorize]
        public async Task<IActionResult> PostAsync([FromBody] UpsertParticipantEntry addEntry)
        {
            var participant = await _participantService.AddParticipantAsync(addEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, participant);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteParticipantRoute)]
        [Authorize]
        public async Task<IActionResult> DeleteAsync(Guid participantId)
        {
            await _participantService.DeleteParticipantAsync(participantId, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpPatch]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetParticipantEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.UpdateParticipantRoute)]
        [Authorize]
        public async Task<IActionResult> PatchAsync(Guid participantId, [FromBody] UpsertParticipantEntry updateEntry)
        {
            var entry = await _participantService.UpdateParticipantAsync(participantId, updateEntry, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, entry);
        }
    }
}
