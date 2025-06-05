using EnsureThat;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaShortEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public string Link { get; set; } = string.Empty;

        public MediaShortEntry(MediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            FileName = media.FileName;
            MediaType = media.MediaType;
            Link = media.Link;
        }
    }
}
