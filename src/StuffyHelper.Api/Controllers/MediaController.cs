using EnsureThat;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.UnsupportedMediaType)]
        [Route(KnownRoutes.StoreMediaFormFileRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> StoreMediaFormFileAsync(
            [FromForm] AddMediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            var slide = await _mediaService.StoreMediaFormFileAsync(
                media,
                cancellationToken: HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        [HttpGet]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Produces(KnownContentTypes.MultipartFormData)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaFromFileRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> RetrieveMediaFormFileAsync(
            Guid mediaId)
        {
            var slide =
                await _mediaService.GetMediaFormFileAsync(
                mediaId,
                HttpContext.RequestAborted);

            return File(slide.Stream, slide.ContentType, $"{slide.FileName}{slide.Ext}");
        }

        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediaMetadataRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMediaMetadataAsync(
            Guid mediaId)
        {
            var slide = await _mediaService.GetMediaMetadataAsync(
                mediaId,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteMediaRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteMediaAsync(
            Guid mediaId)
        {
            await _mediaService.DeleteMediaAsync(
                mediaId,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK);
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        //[Route(KnownRoutes.RetrieveMediaPresignedUrlRoute)]
        //public async Task<IActionResult> GetStudySeriesSlidePresignedUrlAsync(
        //    Guid eventId,
        //    string mediaUid)
        //{
        //    var uri = await _mediaService.ObtainGetMediaPresignedUrl(
        //        eventId,
        //        mediaUid,
        //        HttpContext.RequestAborted);

        //    return StatusCode((int)HttpStatusCode.OK, uri.ToString());
        //}

        //[HttpPost]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        //[Route(KnownRoutes.StoreMediaPresignedUrlRoute)]
        //public async Task<IActionResult> PutStudySeriesSlidePresignedUrlAsync(
        //    Guid eventId,
        //    string mediaUid,
        //    FileType fileType)
        //{
        //    var uri = await _mediaService.ObtainPutMediaPresignedUrl(
        //        eventId,
        //        mediaUid,
        //        fileType,
        //        HttpContext.RequestAborted);

        //    return StatusCode((int)HttpStatusCode.OK, uri.ToString());
        //}

        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<GetMediaEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediasMetadatasRoute)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetMediaMetadatasAsync(
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null)
        {
            var slide = await _mediaService.GetMediaMetadatasAsync(offset, limit, eventId, createdDateStart,
                                                                    createdDateEnd, mediaType, HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }
    }
}
