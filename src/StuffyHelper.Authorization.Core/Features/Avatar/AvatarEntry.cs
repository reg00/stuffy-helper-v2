using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public class AvatarEntry
    {
        public Guid Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public FileType FileType { get; set; }

        public virtual StuffyUser User { get; set; }

        public AvatarEntry()
        {
            User = new();
        }

        public AvatarEntry(
            string userId,
            string fileName,
            FileType fileType)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            UserId = userId;
            FileType = fileType;
            FileName = fileName;
            User = new();
        }

        public void PatchFrom(AddAvatarEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            FileName = Path.GetFileNameWithoutExtension(entry.File!.FileName);
            FileType = FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(entry.File.FileName));
            CreatedDate = DateTime.UtcNow;
        }
    }
}
