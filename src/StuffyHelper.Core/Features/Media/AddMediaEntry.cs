using Microsoft.AspNetCore.Http;
using StuffyHelper.Core.Features.Common;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Media
{
    public class AddMediaEntry
    {
        [Required]
        public Guid EventId { get; set; }
        public IFormFile? File { get; set; }
        [Required]
        public MediaType MediaType { get; set; }
        public string? Link { get; set; }

        public AddMediaEntry()
        {

        }

        public AddMediaEntry(
            Guid eventId,
            IFormFile? file,
            MediaType mediaType,
            string? link)
        {
            EventId = eventId;
            File = file;
            MediaType = mediaType;
            Link = link;
        }

        public MediaEntry ToCommonEntry(bool isPrimal)
        {
            return new MediaEntry(
                EventId,
                File is not null ? Path.GetFileNameWithoutExtension(File.FileName) : null,
                File is not null ? FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(File.FileName)) : FileType.Link,
                MediaType,
                Link,
                isPrimal);
        }
    }
}
