using EnsureThat;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Features.Participant;
using StuffyHelper.Core.Features.Shopping;

namespace StuffyHelper.Core.Features.Event
{
    public class EventEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
        public string UserId { get; set; }
        public bool IsCompleted { get; set; }
        public bool IsActive { get; set; }

        public virtual List<ParticipantEntry> Participants { get; set; } = new List<ParticipantEntry>();
        public virtual List<ShoppingEntry> Shoppings { get; set; } = new List<ShoppingEntry>();
        public virtual List<MediaEntry> Medias { get; set; } = new List<MediaEntry>();

        public void PatchFrom(UpsertEventEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Description = entry.Description;
            EventDateEnd = entry.EventDateEnd;
            EventDateStart = entry.EventDateStart;
            IsCompleted = entry.IsCompleted;
            IsActive = entry.IsActive;
        }
    }
}
