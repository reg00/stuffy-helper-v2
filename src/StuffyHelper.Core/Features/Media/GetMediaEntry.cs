using EnsureThat;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Features.Media
{
    public class GetMediaEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public string Link { get; set; } = string.Empty;
        public EventShortEntry? Event { get; set; }

        public GetMediaEntry(MediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            FileName = media.FileName;
            MediaType = media.MediaType;
            Link = media.Link;
            Event = new EventShortEntry(media.Event);
        }
    }
}
