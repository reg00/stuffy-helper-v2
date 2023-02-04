using EnsureThat;
using Reg00.Infrastructure.Errors;
using Reg00.Infrastructure.Extensions;
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

        public async Task DeleteAvatarAsync(string userId, CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNullOrEmpty(userId, nameof(userId));

            AvatarEntry entry = null;

            try
            {
                entry = await _avatarStore.GetAvatarAsync(
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
                throw new ArgumentNullException("file cannot be null");

            if (!FormFileExtensions.IsImage(avatar.File))
                throw new Reg00.Infrastructure.Errors.NotSupportedException("Is not valid image");

            AvatarEntry entry = null;

            try
            {
                entry = await _avatarStore.GetAvatarAsync(avatar.UserId, cancellationToken);
                entry.PatchFrom(avatar);

                var avatarEntry = await _avatarStore.UpdateAvatarAsync(
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

                var avatarEntry = await _avatarStore.AddAvatarAsync(
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
            catch (EntityNotFoundException)
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
