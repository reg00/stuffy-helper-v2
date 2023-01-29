namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public interface IAvatarStore
    {
        Task<AvatarEntry> AddAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

        Task<AvatarEntry> UpdateAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

        Task DeleteAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

        Task<AvatarEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

        Task<AvatarEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default);
    }
}
