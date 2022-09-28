using EnsureThat;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Media
{
    public class GetMediaEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string MediaUid { get; set; }
        public MediaType MediaType { get; set; }

        public GetEventEntry? Event { get; set; }

        public GetMediaEntry() { }

        public GetMediaEntry(MediaEntry media, bool includeEvent)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            MediaUid = media.MediaUid;
            MediaType = media.MediaType;
            Event = includeEvent ? new GetEventEntry(media.Event, null, false, false, false) : null;
        }
    }
}
