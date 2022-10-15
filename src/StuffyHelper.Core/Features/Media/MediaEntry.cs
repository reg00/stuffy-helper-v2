using EnsureThat;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaEntry
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string FileName { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public FileType FileType { get; set; }
        public MediaType MediaType { get; set; }
        public string? Link { get; set; }
        public bool IsPrimal { get; set; }

        public virtual EventEntry? Event { get; set; }

        public MediaEntry()
        {

        }

        public MediaEntry(
            Guid eventId,
            string mediaUid,
            FileType fileType, 
            MediaType mediaType,
            string link,
            bool isPrimal)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            EventId = eventId;
            FileType = fileType;
            FileName = mediaUid;
            MediaType = mediaType;
            Link = link;
            IsPrimal = isPrimal;
        }
    }
}
