using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public class AvatarEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public string? FileName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public FileType FileType { get; set; }

        public virtual StuffyUser User { get; set; }

        public AvatarEntry()
        {
        }

        public AvatarEntry(
            string userId,
            string? fileName,
            FileType fileType)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            UserId = userId;
            FileType = fileType;
            FileName = fileName;
        }
    }
}
