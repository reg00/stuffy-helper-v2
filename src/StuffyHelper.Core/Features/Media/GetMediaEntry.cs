using EnsureThat;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Core.Features.Event;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Features.Media
{
    public class GetMediaEntry
    {
        public Guid Id { get; init; }
        public FileType FileType { get; init; }
        public string FileName { get; init; } = string.Empty;
        public MediaType MediaType { get; init; }
        public string Link { get; init; } = string.Empty;
        public EventShortEntry? Event { get; init; }

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
