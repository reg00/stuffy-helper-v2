using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class AddEventEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        [Required]
        public DateTime EventDateStart { get; set; }
        public string Description { get; init; } = string.Empty;
        public DateTime? EventDateEnd { get; set; }
    }
}
