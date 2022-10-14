﻿using EnsureThat;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Media
{
    public class GetMediaEntry
    {
        public Guid Id { get; set; }
        public FileType FileType { get; set; }
        public string MediaUid { get; set; }
        public MediaType MediaType { get; set; }
        public string? Link { get; set; }
        public EventShortEntry? Event { get; set; }

        public GetMediaEntry(MediaEntry media)
        {
            EnsureArg.IsNotNull(media, nameof(media));

            Id = media.Id;
            FileType = media.FileType;
            MediaUid = media.MediaUid;
            MediaType = media.MediaType;
            Link = media.Link;
            Event = new EventShortEntry(media.Event);
        }
    }
}
