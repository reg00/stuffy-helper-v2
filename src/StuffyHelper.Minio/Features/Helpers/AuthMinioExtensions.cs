using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Minio.Features.Helpers;

/// <summary>
/// Minio extensions
/// </summary>
public static class AuthMinioExtensions
{
    /// <summary>
    /// Return avatar object name
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="fileType"></param>
    /// <returns></returns>
    public static string GetAvatarObjectName(
        string userId,
        FileType fileType)
    {
        return $"avatars/{userId}.{fileType.ToString().ToLowerInvariant()}";
    }
}