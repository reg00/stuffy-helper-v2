using EnsureThat;
using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Authorization.Core.Services.Interfaces;
using StuffyHelper.Authorization.Data.Repository.Interfaces;
using StuffyHelper.Common.Exceptions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Authorization.Core.Services;

public class AvatarService : IAvatarService
    {
        private readonly IAvatarRepository _avatarRepository;
        private readonly IFileStore _fileStore;

        public AvatarService(IAvatarRepository avatarRepository, IFileStore fileStore)
        {
            _avatarRepository = avatarRepository;
            _fileStore = fileStore;
        }

        public async Task DeleteAvatarAsync(string userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrEmpty(userId, nameof(userId));

            AvatarEntry? entry = null;

            try
            {
                entry = await _avatarRepository.GetAvatarAsync(
                userId,
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
                    await _avatarRepository.DeleteAvatarAsync(
                        entry,
                        cancellationToken);

                    await _fileStore.DeleteFilesIfExistAsync(
                        AuthMinioExtensions.GetAvatarObjectName(
                            entry.UserId, entry.FileType),
                        cancellationToken);
                }
            }
        }

        public async Task<MediaBlobEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(avatarId, nameof(avatarId));

            var entry = await _avatarRepository.GetAvatarAsync(
                avatarId,
                cancellationToken);

            var stream = await _fileStore.GetFileAsync(
                AuthMinioExtensions.GetAvatarObjectName(
                    entry.UserId, entry.FileType),
                cancellationToken);

            return new MediaBlobEntry(stream, entry.FileName, entry.FileType);
        }

        public async Task<AvatarEntry> GetAvatarMetadataAsync(Guid avatarId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(avatarId, nameof(avatarId));

            var entry = await _avatarRepository.GetAvatarAsync(
                avatarId,
                cancellationToken);

            return entry;
        }

        public async Task<AvatarEntry> StoreAvatarFormFileAsync(
            AddAvatarEntry avatar,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(avatar, nameof(avatar));
            EnsureArg.IsNotNullOrWhiteSpace(avatar.UserId, nameof(avatar.UserId));
            EnsureArg.IsNotNull(avatar.File, nameof(avatar.File));

            if (!FormFileExtensions.IsImage(avatar.File))
                throw new NotSupportedException("Is not valid image");

            AvatarEntry? entry = null;

            try
            {
                entry = await _avatarRepository.GetAvatarAsync(avatar.UserId, cancellationToken);
                entry.PatchFrom(avatar);

                var avatarEntry = await _avatarRepository.UpdateAvatarAsync(
                    entry, cancellationToken);

                await _fileStore.StoreFileAsync(
                    AuthMinioExtensions.GetAvatarObjectName(
                        entry.UserId, entry.FileType),
                        avatar.File!.OpenReadStream(),
                    cancellationToken);

                //return new AvatarShortEntry(entry);

                return avatarEntry;
            }
            catch (EntityAlreadyExistsException)
            {
                throw;
            }
            catch (EntityNotFoundException)
            {
                entry = avatar.ToCommonEntry();

                var avatarEntry = await _avatarRepository.AddAvatarAsync(
                    entry, cancellationToken);

                await _fileStore.StoreFileAsync(
                    AuthMinioExtensions.GetAvatarObjectName(
                        avatarEntry.UserId, avatarEntry.FileType),
                        avatar.File!.OpenReadStream(),
                    cancellationToken);

                return avatarEntry;
            }
            catch (Exception)
            {
                if (entry != null)
                {
                    await _avatarRepository.DeleteAvatarAsync(
                        entry,
                        cancellationToken);

                    await _fileStore.DeleteFilesIfExistAsync(
                        AuthMinioExtensions.GetAvatarObjectName(
                            avatar.UserId, entry.FileType),
                        cancellationToken: cancellationToken);
                }

                throw;
            }
        }

        public async Task<Uri?> GetAvatarUri(string userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                var entry = await _avatarRepository.GetAvatarAsync(
                userId,
                cancellationToken);

                return await _fileStore.ObtainGetPresignedUrl(
                    AuthMinioExtensions.GetAvatarObjectName(userId, entry.FileType),
                    cancellationToken);
            }
            catch (EntityNotFoundException)
            {
                return null;
            }
        }
    }