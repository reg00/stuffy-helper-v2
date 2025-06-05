using EnsureThat;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Entities
{
    public class MediaEntry
    {
        public Guid Id { get; init; }
        public Guid EventId { get; init; }
        public string FileName { get; init; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; }
        public FileType FileType { get; init; }
        public MediaType MediaType { get; init; }
        public string Link { get; set; } = string.Empty;
        public bool IsPrimal { get; init; }

        public virtual EventEntry Event { get; init; }

        public MediaEntry()
        {

        }

        public MediaEntry(
            Guid eventId,
            string fileName,
            FileType fileType,
            MediaType mediaType,
            string link,
            bool isPrimal)
        {
            EnsureArg.IsNotDefault(eventId, nameof(eventId));

            EventId = eventId;
            FileType = fileType;
            FileName = fileName;
            MediaType = mediaType;
            Link = link;
            IsPrimal = isPrimal;
        }
    }
}
