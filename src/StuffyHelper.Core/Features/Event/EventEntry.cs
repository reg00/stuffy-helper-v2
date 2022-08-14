using EnsureThat;
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
        public DateTime EventDate { get; set; }
        public string UserId { get; set; }
        public bool IsCompleted { get; set; }

        public virtual List<ParticipantEntry> Participants { get; set; } = new List<ParticipantEntry>();
        public virtual List<ShoppingEntry> Shoppings { get; set; } = new List<ShoppingEntry>();

        public void PatchFrom(UpsertEventEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
            Description = entry.Description;
            EventDate = entry.EventDate;
            IsCompleted = entry.IsCompleted;
        }
    }
}
