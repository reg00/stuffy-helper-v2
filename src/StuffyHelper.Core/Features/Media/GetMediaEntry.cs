using EnsureThat;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Media
{
    public class GetMediaEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string MediaUid { get; set; }

        public GetEventEntry Event { get; set; }

        public GetMediaEntry() { }

        public GetMediaEntry(MediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            MediaUid = media.MediaUid;
            Event = new GetEventEntry(media.Event, null, true, true, true);
        }
    }
}
