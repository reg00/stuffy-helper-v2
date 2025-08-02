using EnsureThat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;

namespace StuffyHelper.Api.Controllers
{
    /// <summary>
    /// Медиа
    /// </summary>
    [Authorize]
    public class MediaController : Controller
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        /// <summary>
        /// Добавление файла к ивенту
        /// </summary>
        [HttpPost]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Consumes(KnownContentTypes.MultipartFormData)]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.UnsupportedMediaType)]
        [Route(KnownRoutes.StoreMediaFormFileRoute)]
        public async Task<IActionResult> StoreMediaFormFileAsync(
            [FromForm] AddMediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            var slide = await _mediaService.StoreMediaFormFileAsync(
                media,
                cancellationToken: HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        /// <summary>
        /// Получение файла
        /// </summary>
        [HttpGet]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Produces(KnownContentTypes.MultipartFormData)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaFromFileRoute)]
        public async Task<IActionResult> RetrieveMediaFormFileAsync(
            Guid mediaId)
        {
            var slide =
                await _mediaService.GetMediaFormFileAsync(
                mediaId,
                HttpContext.RequestAborted);

            return File(slide.Stream, slide.ContentType, $"{slide.FileName}{slide.Ext}");
        }

        /// <summary>
        /// Получение метаданных файла
        /// </summary>
        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediaMetadataRoute)]
        public async Task<IActionResult> GetMediaMetadataAsync(
            Guid mediaId)
        {
            var slide = await _mediaService.GetMediaMetadataAsync(
                mediaId,
                HttpContext.RequestAborted);

            return StatusCode((int)HttpStatusCode.OK, slide);
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteMediaRoute)]
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

        /// <summary>
        /// Получение списка метаданных файлов
        /// </summary>
        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(IEnumerable<GetMediaEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediasMetadatasRoute)]
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
