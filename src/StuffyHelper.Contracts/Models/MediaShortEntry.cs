using EnsureThat;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Models
{
    public class MediaShortEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string FileName { get; set; } = string.Empty;
        public MediaType MediaType { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
