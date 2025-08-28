using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class UnitTypeShortEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}
