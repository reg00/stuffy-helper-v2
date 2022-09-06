using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class UpsertEventEntry
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
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
                EventDateStart = EventDateStart,
                EventDateEnd = EventDateEnd,
                IsCompleted = IsCompleted,
                IsActive = true
            };
        }
    }
}
