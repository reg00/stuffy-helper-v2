using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class AddEventEntry
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public DateTime EventDateStart { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime? EventDateEnd { get; set; }
    }
}
