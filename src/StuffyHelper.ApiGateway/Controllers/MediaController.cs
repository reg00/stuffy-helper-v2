using EnsureThat;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using StuffyHelper.ApiGateway.Core.Services.Interfaces;
using StuffyHelper.Common.Messages;
using StuffyHelper.Common.Web;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.ApiGateway.Controllers
{
    public class MediaController : AuthorizedApiController
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
        [ProducesResponseType(typeof(MediaShortEntry), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.UnsupportedMediaType)]
        [Route(KnownRoutes.StoreMediaFormFileRoute)]
        public async Task<MediaShortEntry> StoreMediaFormFileAsync(
            [FromForm] AddMediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            return await _mediaService.StoreMediaFormFileAsync(Token, media, cancellationToken: HttpContext.RequestAborted);
        }

        /// <summary>
        /// Получение файла
        /// </summary>
        [HttpGet]
        [RequestSizeLimit(int.MaxValue)]
        [RequestFormLimits(MultipartBodyLengthLimit = int.MaxValue)]
        [Produces(KnownContentTypes.MultipartFormData)]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.RetrieveMediaFromFileRoute)]
        public async Task<FileResult> RetrieveMediaFormFileAsync(
            Guid mediaId)
        {
            var fileParam = await _mediaService.GetMediaFormFileAsync(Token, mediaId, HttpContext.RequestAborted);
            var mimeType = MimeTypes.GetMimeType(fileParam.FileName);
            return File(fileParam.Content, mimeType, fileParam.FileName);
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
        public async Task<GetMediaEntry> GetMediaMetadataAsync(
            Guid mediaId)
        {
            return await _mediaService.GetMediaMetadataAsync(Token, mediaId, HttpContext.RequestAborted);
        }

        /// <summary>
        /// Удаление файла
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiError), (int)HttpStatusCode.BadRequest)]
        [Route(KnownRoutes.DeleteMediaRoute)]
        public async Task DeleteMediaAsync(
            Guid mediaId)
        {
            await _mediaService.DeleteMediaAsync(Token, mediaId, HttpContext.RequestAborted);
        }

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
            int offset = 0,
            int limit = 10,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null)
        {
            return await _mediaService.GetMediaMetadatasAsync(Token, offset, limit, eventId, createdDateStart,
                                                                    createdDateEnd, mediaType, HttpContext.RequestAborted);
        }
    }
}
