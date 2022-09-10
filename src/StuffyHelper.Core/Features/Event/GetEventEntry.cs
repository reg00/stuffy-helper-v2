using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
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
        public DateTime EventDateEnd { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

        [Required]
        public GetUserEntry? User { get; set; }
        public List<GetParticipantEntry> Participants { get; set; }
        public List<GetShoppingEntry> Shoppings { get; set; } 



        public GetEventEntry()
        {
            Participants = new List<GetParticipantEntry>();
            Shoppings = new List<GetShoppingEntry>();
        }

        public GetEventEntry(EventEntry entry, GetUserEntry user, bool includeParticipants, bool includeShoppings)
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
            Participants = includeParticipants ? entry.Participants.Select(x => new GetParticipantEntry(x, user, false, false, false)).ToList() : new List<GetParticipantEntry>();
            Shoppings = includeShoppings ? entry.Shoppings.Select(x => new GetShoppingEntry(x, false, false, false)).ToList() : new List<GetShoppingEntry>();
        }
    }
}
