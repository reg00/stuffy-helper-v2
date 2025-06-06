using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class ParticipantShortEntry
    {
        [Required]
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Uri? ImageUri { get; init; }

        public ParticipantShortEntry(ParticipantEntry entry, UserShortEntry? user = null)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = user?.Name ?? string.Empty;
            ImageUri = user?.ImageUri;
        }
    }
}
