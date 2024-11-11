using AutoMapper;
using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Core.Extensions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaService : IMediaService
    {
        private readonly IMediaStore _mediaStore;
        private readonly IFileStore _fileStore;
        private readonly IMapper _mapper;

        public MediaService(IMediaStore mediaStore, IFileStore fileStore, IMapper mapper)
        {
            _mediaStore = mediaStore;
            _fileStore = fileStore;
            _mapper = mapper;
        }

        public async Task DeleteMediaAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            MediaEntry? entry = null;

            try
            {
                entry = await _mediaStore.GetMediaAsync(
                mediaId,
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
                    await _mediaStore.DeleteMediaAsync(
                        entry,
                        cancellationToken);

                    await _fileStore.DeleteFilesIfExistAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            entry.EventId.ToString(),
                            mediaId.ToString(),
                            entry.FileType),
                        cancellationToken);
                }
            }
        }

        public async Task<Uri?> GetEventPrimalMediaUri(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _mediaStore.GetPrimalEventMedia(
                    eventId,
                    cancellationToken);

                return await _fileStore.ObtainGetPresignedUrl(
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
            catch
            {
                throw;
            }
        }

        public async Task<MediaBlobEntry> GetMediaFormFileAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            var entry = await _mediaStore.GetMediaAsync(
                mediaId,
                cancellationToken);

            var stream = await _fileStore.GetFileAsync(
                StuffyMinioExtensions.GetStuffyObjectName(
                    entry.EventId.ToString(),
                    mediaId.ToString(),
                    entry.FileType),
                cancellationToken);

            return _mapper.Map<MediaBlobEntry>((stream, entry));
        }

        public async Task<GetMediaEntry> GetMediaMetadataAsync(Guid mediaId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(mediaId, nameof(mediaId));

            var entry = await _mediaStore.GetMediaAsync(
                mediaId,
                cancellationToken);

            return new GetMediaEntry(entry);
        }

        public async Task<GetMediaEntry?> GetPrimalEventMedia(Guid eventId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            try
            {
                var entry = await _mediaStore.GetPrimalEventMedia(eventId, cancellationToken);
                return new GetMediaEntry(entry);
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<MediaShortEntry>> GetMediaMetadatasAsync(
            int offset,
            int limit,
            Guid? eventId = null,
            DateTimeOffset? createdDateStart = null,
            DateTimeOffset? createdDateEnd = null,
            MediaType? mediaType = null,
            CancellationToken cancellationToken = default)
        {
            try
            {
                return (await _mediaStore.GetMediasAsync(
                offset, limit, eventId, createdDateStart, createdDateEnd, mediaType, cancellationToken))
                .Select(s => new MediaShortEntry(s));
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

        //    var entry = await _mediaStore.GetMediaAsync(
        //       eventId,
        //       mediaUid,
        //       cancellationToken);

        //    return await _fileStore.ObtainGetPresignedUrl(
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
        //        await _mediaStore.AddMediaAsync(entry, cancellationToken);

        //        return await _fileStore.ObtainPutPresignedUrl(
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
        //        await _mediaStore.DeleteMediaAsync(
        //            entry,
        //            cancellationToken);

        //        await _fileStore.DeleteFilesIfExistAsync(
        //            eventId.ToString(),
        //            mediaUid,
        //            fileType,
        //            cancellationToken: cancellationToken);

        //        throw;
        //    }
        //}

        public async Task<MediaShortEntry> StoreMediaFormFileAsync(
            AddMediaEntry media,
            bool isPrimal = false,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(media, nameof(media));
            EnsureArg.IsNotDefault(media.EventId, nameof(media.EventId));

            if (media.MediaType == MediaType.Link && string.IsNullOrWhiteSpace(media.Link))
                throw new ArgumentNullException("link cannot be null");
            else if (media.MediaType != MediaType.Link && media.File is null)
                throw new ArgumentNullException("file cannot be null");

            var entry = media.ToCommonEntry(isPrimal);

            try
            {
                var mediaEntry = await _mediaStore.AddMediaAsync(
                    entry, cancellationToken);

                if (media.MediaType != MediaType.Link)
                    await _fileStore.StoreFileAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            entry.EventId.ToString(),
                            mediaEntry.Id.ToString(),
                            entry.FileType),
                            media.File!.OpenReadStream(),
                        cancellationToken);

                return new MediaShortEntry(entry);
            }
            catch (EntityAlreadyExistsException)
            {
                throw;
            }
            catch (Exception)
            {
                await _mediaStore.DeleteMediaAsync(
                    entry,
                    cancellationToken);

                if (media.MediaType != MediaType.Link)
                    await _fileStore.DeleteFilesIfExistAsync(
                        StuffyMinioExtensions.GetStuffyObjectName(
                            media.EventId.ToString(),
                            entry.Id.ToString(),
                            entry.FileType),
                        cancellationToken: cancellationToken);

                throw;
            }
        }
    }
}
