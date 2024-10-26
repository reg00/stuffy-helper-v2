using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Data.Repository.Interfaces;

public interface IAvatarRepository
{
    Task<AvatarEntry> AddAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    Task<AvatarEntry> UpdateAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    Task DeleteAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    Task<AvatarEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

    Task<AvatarEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default);
}