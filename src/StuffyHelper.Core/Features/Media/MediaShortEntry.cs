using EnsureThat;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaShortEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string MediaUid { get; set; }
        public MediaType MediaType { get; set; }
        public string? Link { get; set; }

        public MediaShortEntry(MediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            MediaUid = media.MediaUid;
            MediaType = media.MediaType;
            Link = media.Link;
        }
    }
}
