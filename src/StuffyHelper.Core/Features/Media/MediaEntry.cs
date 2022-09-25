using EnsureThat;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaEntry
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string MediaUid { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public FileType FileType { get; set; }

        public virtual EventEntry Event { get; set; }

        public MediaEntry() 
        {
        }

        public MediaEntry(Guid eventId, string mediaUid, FileType fileType)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            EventId = eventId;
            FileType = fileType;
            MediaUid = mediaUid;
        }
    }
}
