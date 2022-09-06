using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class UpsertEventEntry
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime EventDate { get; set; }
        public bool IsCompleted { get; set; }
        [Required]
        public string UserId { get; set; }
        public bool IsActive { get; set; }


        public EventEntry ToCommonEntry()
        {
            return new EventEntry()
            {
                Name = Name,
                Description = Description,
                CreatedDate = DateTime.UtcNow,
                UserId = UserId,
                EventDate = EventDate,
                IsCompleted = IsCompleted,
                IsActive = true
            };
        }
    }
}
