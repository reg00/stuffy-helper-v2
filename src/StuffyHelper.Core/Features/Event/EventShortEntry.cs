﻿using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class EventShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime? EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set;}
        [Required]
        public bool IsCompleted { get; set; }
        public Uri? ImageUri { get; set; }

        public EventShortEntry(EventEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Description = entry.Description;
            EventDateStart = entry.EventDateStart;
            EventDateEnd = entry.EventDateEnd;
            IsCompleted = entry.IsCompleted;
            ImageUri = entry.ImageUri;
        }
    }
}
