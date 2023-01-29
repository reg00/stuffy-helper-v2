using Microsoft.AspNetCore.Http;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Authorization.Core.Features.Avatar
{
    public class AddAvatarEntry
    {
        public string UserId { get; set; }
        public IFormFile? File { get; set; }

        public AddAvatarEntry()
        {

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
            return new AvatarEntry(
            UserId,
            File is not null ? Path.GetFileNameWithoutExtension(File.FileName) : null,
            File is not null ? FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(File.FileName)) : FileType.Link);
        }
    }
}
