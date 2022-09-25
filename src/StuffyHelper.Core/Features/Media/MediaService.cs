﻿using EnsureThat;
using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Common;
using EntityAlreadyExistsException = StuffyHelper.Core.Exceptions.EntityAlreadyExistsException;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaService : IMediaService
    {
        private readonly IMediaStore _mediaStore;
        private readonly IFileStore _fileStore;

        public MediaService(IMediaStore mediaStore, IFileStore fileStore)
        {
            _mediaStore = mediaStore;
            _fileStore = fileStore;
        }

        public async Task DeleteMediaAsync(Guid eventId, string mediaUid, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            MediaEntry entry = null;

            try
            {
                entry = await _mediaStore.GetMediaAsync(
                eventId,
                mediaUid,
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
                        eventId.ToString(),
                        mediaUid,
                        entry.FileType,
                        cancellationToken);
                }
            }
        }

        public async Task<MediaBlobEntry> GetMediaFormFileAsync(Guid eventId, string mediaUid, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            var entry = await _mediaStore.GetMediaAsync(
                eventId,
                mediaUid,
                cancellationToken);

            var stream = await _fileStore.GetFileAsync(
                eventId.ToString(),
                mediaUid,
                entry.FileType,
                cancellationToken);

            return new MediaBlobEntry(stream, entry.FileType);
        }

        public async Task<GetMediaEntry> GetMediaMetadataAsync(Guid eventId, string mediaUid, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            var entry = await _mediaStore.GetMediaAsync(
                eventId,
                mediaUid,
                cancellationToken);

            return new GetMediaEntry(entry);
        }

        public async Task<IEnumerable<GetMediaEntry>> GetMediaMetadatasAsync(int offset, int limit, Guid? eventId = null, DateTimeOffset? createdDateStart = null, DateTimeOffset? createdDateEnd = null, CancellationToken cancellationToken = default)
        {
            try
            {
                return (await _mediaStore.GetMediasAsync(
                offset, limit, eventId, createdDateStart, createdDateEnd, cancellationToken))
                .Select(s => new GetMediaEntry(s));
            }
            catch (ResourceNotFoundException)
            {
                return new List<GetMediaEntry>();
            }
        }

        public async Task<Uri> ObtainGetMediaPresignedUrl(Guid eventId, string mediaUid, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            var entry = await _mediaStore.GetMediaAsync(
               eventId,
               mediaUid,
               cancellationToken);

            return await _fileStore.ObtainGetPresignedUrl(
                eventId.ToString(),
                mediaUid,
                entry.FileType,
                cancellationToken);
        }

        public async Task<Uri> ObtainPutMediaPresignedUrl(Guid eventId, string mediaUid, FileType fileType, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            var entry = new MediaEntry(eventId, mediaUid, fileType);

            try
            {
                await _mediaStore.AddMediaAsync(entry, cancellationToken);

                return await _fileStore.ObtainPutPresignedUrl(
                eventId.ToString(),
                mediaUid,
                fileType,
                cancellationToken);
            }
            catch (EntityAlreadyExistsException)
            {
                throw;
            }
            catch (ResourceNotFoundException)
            {
                throw;
            }
            catch (Exception)
            {
                await _mediaStore.DeleteMediaAsync(
                    entry,
                    cancellationToken);

                await _fileStore.DeleteFilesIfExistAsync(
                    eventId.ToString(),
                    mediaUid,
                    fileType,
                    cancellationToken: cancellationToken);

                throw;
            }
        }

        public async Task<GetMediaEntry> StoreMediaFormFileAsync(Guid eventId, string mediaUid, FileType fileType, Stream requestStream, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));
            EnsureArg.IsNotNullOrWhiteSpace(mediaUid, nameof(mediaUid));

            var entry = new MediaEntry(eventId, mediaUid, fileType);

            try
            {
                var mediaEntry = await _mediaStore.AddMediaAsync(
                    entry, cancellationToken);

                await _fileStore.StoreFileAsync(
                    eventId.ToString(),
                    mediaUid,
                    requestStream,
                    entry.FileType,
                    cancellationToken);

                return new GetMediaEntry(entry);
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

                await _fileStore.DeleteFilesIfExistAsync(
                    eventId.ToString(),
                    entry.Id.ToString(),
                    entry.FileType,
                    cancellationToken: cancellationToken);

                throw;
            }
        }
    }
}
