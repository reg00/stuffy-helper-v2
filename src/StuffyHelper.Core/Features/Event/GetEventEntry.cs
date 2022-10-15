using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Shopping;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class GetEventEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime CreatedDate { get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public UserShortEntry? User { get; set; }
        public List<ParticipantShortEntry> Participants { get; set; }
        public List<ShoppingShortEntry> Shoppings { get; set; } 
        public List<MediaShortEntry> Medias { get; set; } 



        public GetEventEntry()
        {
            Participants = new List<ParticipantShortEntry>();
            Shoppings = new List<ShoppingShortEntry>();
            Medias = new List<MediaShortEntry>();
        }

        public GetEventEntry(EventEntry entry, UserShortEntry user, List<ParticipantShortEntry> participants = null)
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
            Shoppings = entry.Shoppings.Select(x => new ShoppingShortEntry(x)).ToList();
            Medias = entry.Medias.Select(x => new MediaShortEntry(x)).ToList();
        }
    }
}
