using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class UpdateEventEntry
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        public DateTime EventDateEnd { get; set; }
        [Required]
        public bool IsCompleted { get; set; }
        [Required]
        public bool IsActive { get; set; }


        public EventEntry ToCommonEntry(string userId)
        {
            EnsureArg.IsNotNullOrWhiteSpace(userId, nameof(userId));

            return new EventEntry()
            {
                Name = Name,
                Description = Description,
                CreatedDate = DateTime.UtcNow,
                UserId = userId,
                EventDateStart = EventDateStart,
                EventDateEnd = EventDateEnd,
                IsCompleted = IsCompleted,
                IsActive = true
            };
        }
    }
}
