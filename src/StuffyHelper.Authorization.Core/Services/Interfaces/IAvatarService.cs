using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

public interface IAvatarService
{
    Task DeleteAvatarAsync(string userId, CancellationToken cancellationToken = default);

    Task<MediaBlobEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

    Task<Uri?> GetAvatarUri(string userId, CancellationToken cancellationToken = default);

    Task<AvatarEntry> GetAvatarMetadataAsync(Guid avatarId, CancellationToken cancellationToken = default);

    Task<AvatarEntry> StoreAvatarFormFileAsync(
        AddAvatarEntry avatar,
        CancellationToken cancellationToken = default);
}