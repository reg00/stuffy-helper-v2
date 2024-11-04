using StuffyHelper.Authorization.Contracts.Entities;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Services.Interfaces;

/// <summary>
/// Avatar service
/// </summary>
public interface IAvatarService
{
    /// <summary>
    /// Delete avatar from our database
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAvatarAsync(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return avatar entry by avatar id
    /// </summary>
    /// <param name="avatarId">Avatar id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<MediaBlobEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return avatar uri by user id
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<Uri?> GetAvatarUri(string userId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return avatar metadata from our database
    /// </summary>
    /// <param name="avatarId">Avatar id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> GetAvatarMetadataAsync(Guid avatarId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Add or update users avatar
    /// </summary>
    /// <param name="avatar">Add avatar entry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> StoreAvatarFormFileAsync(
        AddAvatarEntry avatar,
        CancellationToken cancellationToken = default);
}