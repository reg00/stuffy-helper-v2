using EnsureThat;
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
    public class MediaController : AuthorizedApiController
    {
        private readonly IMediaService _mediaService;

        /// <summary>
        /// Ctor.
        /// </summary>
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
        [ProducesResponseType(typeof(MediaShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.UnsupportedMediaType)]
        [Route(KnownRoutes.StoreMediaFormFileRoute)]
        public async Task<MediaShortEntry> StoreMediaFormFileAsync([FromRoute] Guid eventId,
            [FromForm] AddMediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            return await _mediaService.StoreMediaFormFileAsync(
                eventId, media,
                cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение файла
        /// </summary>
        [HttpGet]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Produces(KnownContentTypes.MultipartFormData)]
        [ProducesResponseType(typeof(FileResult), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaFromFileRoute)]
        public async Task<FileResult> RetrieveMediaFormFileAsync([FromRoute] Guid eventId,
            Guid mediaId)
        {
            var slide =
                await _mediaService.GetMediaFormFileAsync(
                eventId, mediaId,
                HttpContext.RequestAborted);

            return File(slide.Stream, slide.ContentType, $"{slide.FileName}{slide.Ext}");
        }

        /// <summary>
        /// Получение метаданных файла
        /// </summary>
        [HttpGet]
        [Produces(KnownContentTypes.ApplicationJson)]
        [ProducesResponseType(typeof(GetMediaEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediaMetadataRoute)]
        public async Task<GetMediaEntry> GetMediaMetadataAsync([FromRoute] Guid eventId,
            Guid mediaId)
        {
            return await _mediaService.GetMediaMetadataAsync(
                eventId, mediaId,
                HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteMediaRoute)]
        public async Task DeleteMediaAsync([FromRoute] Guid eventId,
            Guid mediaId)
        {
            await _mediaService.DeleteMediaAsync(
                eventId, mediaId,
                HttpContext.RequestAborted);
        }

        //[HttpGet]
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
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
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.OK)]
        //[ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
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
        [ProducesResponseType(typeof(IEnumerable<MediaShortEntry>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.GetMediasMetadatasRoute)]
        public async Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            [FromRoute] Guid eventId,
            int offset = 0,
            int limit = 10,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null)
        {
            return await _mediaService.GetMediaMetadatasAsync(eventId, offset, limit, createdDateStart,
                                                                    createdDateEnd, mediaType, HttpContext.RequestAborted);
        }
    }
}
