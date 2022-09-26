using EnsureThat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StuffyHelper.Api.Web;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Core.Features.Media;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace StuffyHelper.Api.Controllers
{
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.UnsupportedMediaType)]
        [Route(KnownRoutes.StoreMediaFormFileRoute)]
        public async Task<IActionResult> StoreMediaFormFileAsync(
            IFormFile file,
            [FromRoute] Guid eventId)
        {
            EnsureArg.IsNotNull(file, nameof(file));

            var slide = await _mediaService.StoreMediaFormFileAsync(
                eventId,
                Path.GetFileNameWithoutExtension(file.FileName),
                FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(file.FileName)),
                file.OpenReadStream(),
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        [HttpGet]
        //[DisableFormValueModelBinding]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Produces(KnownContentTypes.MultipartFormData)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaFromFileRoute)]
        public async Task<IActionResult> RetrieveMediaFormFileAsync(
            Guid eventId,
            string mediaUid)
        {
            var slide =
                await _mediaService.GetMediaFormFileAsync(
                eventId,
                mediaUid,
                HttpContext.RequestAborted);

            return File(slide.Stream, slide.ContentType, $"{mediaUid}{slide.Ext}");
        }

        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediaMetadataRoute)]
        public async Task<IActionResult> GetMediaMetadataAsync(
            Guid eventId,
            string mediaUid)
        {
            var slide = await _mediaService.GetMediaMetadataAsync(
                eventId,
                mediaUid,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteMediaRoute)]
        public async Task<IActionResult> DeleteStudySeriesSlideAsync(
            Guid eventId,
            string mediaUid)
        {
            await _mediaService.DeleteMediaAsync(
                eventId,
                mediaUid,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        [HttpGet]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaPresignedUrlRoute)]
        public async Task<IActionResult> GetStudySeriesSlidePresignedUrlAsync(
            Guid eventId,
            string mediaUid)
        {
            var uri = await _mediaService.ObtainGetMediaPresignedUrl(
                eventId,
                mediaUid,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, uri.ToString());
        }

        [HttpPost]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.StoreMediaPresignedUrlRoute)]
        public async Task<IActionResult> PutStudySeriesSlidePresignedUrlAsync(
            Guid eventId,
            string mediaUid,
            FileType fileType)
        {
            var uri = await _mediaService.ObtainPutMediaPresignedUrl(
                eventId,
                mediaUid,
                fileType,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, uri.ToString());
        }

        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<GetMediaEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediasMetadatasRoute)]
        public async Task<IActionResult> GetSlidesMetadataAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null)
        {
            var slide = await _mediaService.GetMediaMetadatasAsync(offset, limit, eventId, createdDateStart,
                                                                    createdDateEnd, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }
    }
}
