﻿using Microsoft.AspNetCore.Http;
using StuffyHelper.Minio.Features.Common;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Media
{
    public class AddMediaEntry
    {
        [Required]
        public Guid EventId { get; init; }
        public IFormFile? File { get; set; }
        [Required]
        public MediaType MediaType { get; init; }
        public string Link { get; init; } = string.Empty;

        public AddMediaEntry()
        {

        }

        public AddMediaEntry(
            Guid eventId,
            IFormFile? file,
            MediaType mediaType,
            string link)
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
                File is not null ? Path.GetFileNameWithoutExtension(File.FileName) : string.Empty,
                File is not null ? FileTypeMapper.MapFileTypeFromExt(Path.GetExtension(File.FileName)) : FileType.Link,
                MediaType,
                Link,
                isPrimal);
        }
    }
}
