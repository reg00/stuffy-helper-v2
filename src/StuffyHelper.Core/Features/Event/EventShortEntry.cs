using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class EventShortEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? EventDateStart { get; set; }
        [Required]
        public bool IsCompleted { get; set; }

        public EventShortEntry(EventEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Description = entry.Description;
            EventDateStart = entry.EventDateStart;
            IsCompleted = entry.IsCompleted;
        }
    }
}
