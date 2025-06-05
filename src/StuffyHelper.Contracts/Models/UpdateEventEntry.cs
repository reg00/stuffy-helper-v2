using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class UpdateEventEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        [Required]
        public DateTime EventDateStart { get; set; }
        public DateTime? EventDateEnd { get; set; }
    }
}
