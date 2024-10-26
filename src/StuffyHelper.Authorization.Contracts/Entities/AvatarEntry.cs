using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Authorization.Contracts.Entities;

public class AvatarEntry
{
    public Guid Id { get; init; }
    public string UserId { get; init; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
    public FileType FileType { get; set; }
    public DateTimeOffset CreatedDate { get; set; }

    public virtual StuffyUser User { get; set; }

    public AvatarEntry()
    {
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
    }

    public void PatchFrom(AddAvatarEntry entry)
    {
        EnsureArg.IsNotNull(entry, nameof(entry));

        FileName = Path.GetFileNameWithoutExtension(entry.File!.FileName);
        FileType = FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(entry.File.FileName));
        CreatedDate = DateTime.UtcNow;
    }
}