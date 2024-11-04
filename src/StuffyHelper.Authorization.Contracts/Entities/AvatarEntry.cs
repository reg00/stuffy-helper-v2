using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Minio.Features.Common;
using StuffyHelper.Minio.Features.Helpers;

namespace StuffyHelper.Authorization.Contracts.Entities;

/// <summary>
/// Record for work with user avatar
/// </summary>
public class AvatarEntry
{
    /// <summary>
    /// Identifier
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    /// Users id
    /// </summary>
    public string UserId { get; init; } = string.Empty;
    
    /// <summary>
    /// File name
    /// </summary>
    public string FileName { get; set; } = string.Empty;
    
    /// <summary>
    /// File type
    /// </summary>
    public FileType FileType { get; set; }
    
    /// <summary>
    /// Created date
    /// </summary>
    public DateTimeOffset CreatedDate { get; set; }

    /// <summary>
    /// Linked user entity
    /// </summary>
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