using EnsureThat;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public class AddAvatarEntry
    {
        public string UserId { get; set; } = string.Empty;
        public IFormFile? File { get; set; }

        public AddAvatarEntry()
        {
            File = null;
        }

        public AddAvatarEntry(
            string userId,
            IFormFile? file)
        {
            UserId = userId;
            File = file;
        }

        public AvatarEntry ToCommonEntry()
        {
            EnsureArg.IsNotNull(File, nameof(File));

            return new AvatarEntry(
            UserId,
            Path.GetFileNameWithoutExtension(File.FileName),
            FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(File.FileName)));
        }
    }
}
