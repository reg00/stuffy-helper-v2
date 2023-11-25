using EnsureThat;
using StuffyHelper.Authorization.Core.Models.User;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class GetEventEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        public Uri? MediaUri { get; set; }


        [Required]
        public UserShortEntry? User { get; set; }
        public List<ParticipantShortEntry> Participants { get; set; }
        public List<PurchaseShortEntry> Purchases { get; set; }
        public List<MediaShortEntry> Medias { get; set; }


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
