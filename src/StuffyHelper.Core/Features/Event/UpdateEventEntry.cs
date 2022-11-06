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
    }
}
