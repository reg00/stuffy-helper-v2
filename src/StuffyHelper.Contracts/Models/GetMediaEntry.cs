using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Models
{
    public class GetMediaEntry
    {
        public Guid Id { get; init; }
        public FileType FileType { get; init; }
        public string FileName { get; init; } = string.Empty;
        public MediaType MediaType { get; init; }
        public string Link { get; init; } = string.Empty;
        public EventShortEntry? Event { get; init; }
    }
}
