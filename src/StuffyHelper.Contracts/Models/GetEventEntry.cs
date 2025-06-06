using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class GetEventEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        [Required]
        public DateTime CreatedDate { get; init; }
        [Required]
        public DateTime EventDateStart { get; init; }
        public DateTime? EventDateEnd { get; init; }
        [Required]
        public bool IsCompleted { get; init; }
        public Uri? MediaUri { get; init; }


        [Required]
        public UserShortEntry? User { get; init; }
        public List<ParticipantShortEntry> Participants { get; init; }
        public List<PurchaseShortEntry> Purchases { get; init; }
        public List<MediaShortEntry> Medias { get; init; }


        public GetEventEntry()
        {
            Participants = new List<ParticipantShortEntry>();
            Purchases = new List<PurchaseShortEntry>();
            Medias = new List<MediaShortEntry>();
        }

        public GetEventEntry(
            EventEntry entry,
            UserShortEntry user,
            List<ParticipantShortEntry> participants)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Description = entry.Description;
            CreatedDate = entry.CreatedDate;
            EventDateEnd = entry.EventDateEnd;
            EventDateStart = entry.EventDateStart;
            IsCompleted = entry.IsCompleted;
            User = user;
            Participants = participants;
            MediaUri = entry.ImageUri;
            Purchases = entry.Purchases.Select(x => new PurchaseShortEntry(x)).ToList();
            Medias = entry.Medias.Select(x => new MediaShortEntry(x)).ToList();
        }
    }
}
