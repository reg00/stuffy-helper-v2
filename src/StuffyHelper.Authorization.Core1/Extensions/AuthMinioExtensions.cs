using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core1.Extensions
{
    public static class AuthMinioExtensions
    {
        public static string GetAvatarObjectName(
            string userId,
            FileType fileType)
        {
            return $"avatars/{userId}.{fileType.ToString().ToLowerInvariant()}";
        }
    }
}
