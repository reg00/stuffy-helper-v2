using EnsureThat;
using Microsoft.Extensions.Logging;
using StuffyHelper.Authorization.Core.Exceptions;
using StuffyHelper.Authorization.Core.Extensions;
using StuffyHelper.Core.Features.Common;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public class AvatarService : IAvatarService
    {
        private readonly IAvatarStore _avatarStore;
        private readonly IFileStore _fileStore;

        public AvatarService(IAvatarStore avatarStore, IFileStore fileStore)
        {
            _avatarStore = avatarStore;
            _fileStore = fileStore;
        }

        public async Task DeleteAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotDefault(avatarId, nameof(avatarId));

            AvatarEntry entry = null;

            try
            {
                entry = await _avatarStore.GetAvatarAsync(
                avatarId,
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
                    await _avatarStore.DeleteAvatarAsync(
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

            var entry = await _avatarStore.GetAvatarAsync(
                avatarId,
                cancellationToken);

            var stream = await _fileStore.GetFileAsync(
                AuthMinioExtensions.GetAvatarObjectName(
                    entry.UserId, entry.FileType),
                cancellationToken);

            return new MediaBlobEntry(stream, entry.FileName, entry.FileType);
        }

        public async Task<MediaBlobEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            var entry = await _avatarStore.GetAvatarAsync(
                userId,
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

            var entry = await _avatarStore.GetAvatarAsync(
                avatarId,
                cancellationToken);

            //return new GetAvatarEntry(entry);
            return entry;
        }

        public async Task<AvatarEntry> StoreAvatarFormFileAsync(
            AddAvatarEntry avatar,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(avatar, nameof(avatar));
            EnsureArg.IsNotNullOrWhiteSpace(avatar.UserId, nameof(avatar.UserId));

            if (avatar.File is null)
                throw new AuthStoreException("file cannot be null");

            var entry = avatar.ToCommonEntry();

            try
            {
                var avatarEntry = await _avatarStore.AddAvatarAsync(
                    entry, cancellationToken);

                await _fileStore.StoreFileAsync(
                    AuthMinioExtensions.GetAvatarObjectName(
                        entry.UserId, entry.FileType),
                        avatar.File!.OpenReadStream(),
                    cancellationToken);

                //return new AvatarShortEntry(entry);

                return entry;
            }
            catch (AuthorizationEntityAlreadyExistsException)
            {
                throw;
            }
            catch (Exception)
            {
                await _avatarStore.DeleteAvatarAsync(
                    entry,
                    cancellationToken);

                await _fileStore.DeleteFilesIfExistAsync(
                    AuthMinioExtensions.GetAvatarObjectName(
                        avatar.UserId, entry.FileType),
                    cancellationToken: cancellationToken);

                throw;
            }
        }

        public async Task<Uri> GetAvatarUri(string userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            try
            {
                var entry = await _avatarStore.GetAvatarAsync(
                userId,
                cancellationToken);

                return await _fileStore.ObtainGetPresignedUrl(
                    AuthMinioExtensions.GetAvatarObjectName(userId, entry.FileType),
                    cancellationToken);
            }
            catch (AuthorizationResourceNotFoundException)
            {
                return null;
            }
            catch
            {
                throw;
            }
        }
    }
}
