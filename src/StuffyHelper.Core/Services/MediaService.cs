using AutoMapper;
using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;
using StuffyHelper.Minio.Extensions;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Storage;

namespace StuffyHelper.Core.Services
{
    /// <inheritdoc />
    public class MediaService : IMediaService
    {
        private readonly IMediaRepository _mediaRepository;
        private readonly IFileStore _fileRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Ctor.
        /// </summary>
        public MediaService(IMediaRepository mediaRepository, IFileStore fileRepository, IMapper mapper)
        {
            _mediaRepository = mediaRepository;
            _fileRepository = fileRepository;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task DeleteMediaAsync(Guid eventId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            MediaEntry? entry = null;

            try
            {
                entry = await _mediaRepository.GetMediaAsync(
                eventId, mediaId,
                cancellationToken);
            }
            catch (Exception)
            {
                // Fire and forget
            }
            finally
            {
                if (entry is not null)
                {
                    await _mediaRepository.DeleteMediaAsync(
                        entry,
                        cancellationToken);

                    await _fileRepository.DeleteFilesIfExistAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            entry.EventId.ToString(),
                            mediaId.ToString(),
                            entry.FileType),
                        cancellationToken);
                }
            }
        }

        /// <inheritdoc />
        public async Task<Uri?> GetEventPrimalMediaUri(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _mediaRepository.GetPrimalEventMedia(
                    eventId,
                    cancellationToken);

                if (entry == null)
                    return null;
                
                return await _fileRepository.ObtainGetPresignedUrl(
                    StuffyMinioExtensions.GetStuffyObjectName(
                        entry.EventId.ToString(),
                        entry.Id.ToString(),
                        entry.FileType),
                    cancellationToken);
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
        }

        /// <inheritdoc />
        public async Task<MediaBlobEntry> GetMediaFormFileAsync(Guid eventId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            var entry = await _mediaRepository.GetMediaAsync(
                eventId, mediaId,
                cancellationToken);

            var stream = await _fileRepository.GetFileAsync(
                StuffyMinioExtensions.GetStuffyObjectName(
                    entry.EventId.ToString(),
                    mediaId.ToString(),
                    entry.FileType),
                cancellationToken);

            return _mapper.Map<MediaBlobEntry>((stream, entry));
        }

        /// <inheritdoc />
        public async Task<GetMediaEntry> GetMediaMetadataAsync(Guid eventId, Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            var entry = await _mediaRepository.GetMediaAsync(
                eventId, mediaId,
                cancellationToken);

            return _mapper.Map<GetMediaEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<GetMediaEntry?> GetPrimalEventMedia(Guid eventId,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            var entry = await _mediaRepository.GetPrimalEventMedia(eventId, cancellationToken);

            return _mapper.Map<GetMediaEntry>(entry);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            Guid eventId, 
            int offset,
            int limit,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return (await _mediaRepository.GetMediasAsync(eventId,
                offset, limit, createdDateStart, createdDateEnd, mediaType, cancellationToken))
                .Select(s => _mapper.Map<MediaShortEntry>(s));
            }
            catch (EntityNotFoundException)
            {
                return new List<MediaShortEntry>();
            }
        }

        //public async Task<Uri> ObtainGetMediaPresignedUrl(Guid eventId, string mediaUid, CancellationToken cancellationToken = default)
        //{
        //    EnsureArg.IsNotDefault(eventId, nameof(eventId));
        //    EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

        //    var entry = await _mediaRepository.GetMediaAsync(
        //       eventId,
        //       mediaUid,
        //       cancellationToken);

        //    return await _fileRepository.ObtainGetPresignedUrl(
        //        eventId.ToString(),
        //        mediaUid,
        //        entry.FileType,
        //        cancellationToken);
        //}

        //public async Task<Uri> ObtainPutMediaPresignedUrl(Guid eventId, string mediaUid, FileType fileType, CancellationToken cancellationToken = default)
        //{
        //    EnsureArg.IsNotDefault(eventId, nameof(eventId));
        //    EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

        //    var entry = new MediaEntry(eventId, mediaUid, fileType);

        //    try
        //    {
        //        await _mediaRepository.AddMediaAsync(entry, cancellationToken);

        //        return await _fileRepository.ObtainPutPresignedUrl(
        //        eventId.ToString(),
        //        mediaUid,
        //        fileType,
        //        cancellationToken);
        //    }
        //    catch (EntityAlreadyExistsException)
        //    {
        //        throw;
        //    }
        //    catch (EntityNotFoundException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        await _mediaRepository.DeleteMediaAsync(
        //            entry,
        //            cancellationToken);

        //        await _fileRepository.DeleteFilesIfExistAsync(
        //            eventId.ToString(),
        //            mediaUid,
        //            fileType,
        //            cancellationToken: cancellationToken);

        //        throw;
        //    }
        //}

        /// <inheritdoc />
        public async Task<MediaShortEntry> StoreMediaFormFileAsync(
            Guid eventId, 
            AddMediaEntry media,
            bool isPrimal = false,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(media, nameof(media));
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            if (media.MediaType == MediaType.Link && string.IsNullOrWhiteSpace(media.Link))
                throw new ArgumentNullException(nameof(media.Link), "link cannot be null");
            if (media.MediaType != MediaType.Link && media.File is null)
                throw new ArgumentNullException(nameof(media.File), "file cannot be null");

            var entry = _mapper.Map<MediaEntry>((media, isPrimal));
            entry.EventId = eventId;
            
            try
            {
                var mediaEntry = await _mediaRepository.AddMediaAsync(
                    entry, cancellationToken);

                if (media.MediaType != MediaType.Link)
                    await _fileRepository.StoreFileAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            entry.EventId.ToString(),
                            mediaEntry.Id.ToString(),
                            entry.FileType),
                            media.File!.OpenReadStream(),
                        cancellationToken);

                return _mapper.Map<MediaShortEntry>(entry);
            }
            catch (EntityAlreadyExistsException)
            {
                throw;
            }
            catch (Exception)
            {
                await _mediaRepository.DeleteMediaAsync(
                    entry,
                    cancellationToken);

                if (media.MediaType != MediaType.Link)
                    await _fileRepository.DeleteFilesIfExistAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            eventId.ToString(),
                            entry.Id.ToString(),
                            entry.FileType),
                        cancellationToken: cancellationToken);

                throw;
            }
        }
    }
}
