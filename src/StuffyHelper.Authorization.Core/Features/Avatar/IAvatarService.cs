using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public interface IAvatarService
    {
        Task DeleteAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);
        
        Task<MediaBlobEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

        Task<MediaBlobEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default);

        Task<Uri> GetAvatarUri(string userId, CancellationToken cancellationToken = default);

        Task<AvatarEntry> GetAvatarMetadataAsync(Guid avatarId, CancellationToken cancellationToken = default);

        Task<AvatarEntry> StoreAvatarFormFileAsync(
            AddAvatarEntry avatar,
            CancellationToken cancellationToken = default);
    }
}
