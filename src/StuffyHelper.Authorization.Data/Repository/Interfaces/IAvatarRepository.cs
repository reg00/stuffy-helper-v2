using StuffyHelper.Authorization.Contracts.Entities;

namespace StuffyHelper.Authorization.Data.Repository.Interfaces;

/// <summary>
/// Interface for work with avatars in our database
/// </summary>
public interface IAvatarRepository
{
    /// <summary>
    /// Add avatar for user in our database
    /// </summary>
    /// <param name="avatar">Avatar entry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> AddAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    /// <summary>
    /// Update avatar for user in our database
    /// </summary>
    /// <param name="avatar">Avatar entry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> UpdateAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete avatar from our database
    /// </summary>
    /// <param name="avatar">Avatar entry</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task DeleteAvatarAsync(AvatarEntry avatar, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return avatar entry by avatar id
    /// </summary>
    /// <param name="avatarId">Avatar id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> GetAvatarAsync(Guid avatarId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Return avatar entry by user id
    /// </summary>
    /// <param name="userId">Avatar id</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AvatarEntry> GetAvatarAsync(string userId, CancellationToken cancellationToken = default);
}