﻿using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Participant
{
    public class ParticipantShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Uri? ImageUri { get; set; }

        public ParticipantShortEntry(ParticipantEntry entry, UserShortEntry? user = null)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = user?.Name ?? string.Empty;
            ImageUri = user?.ImageUri;
        }
    }
}
