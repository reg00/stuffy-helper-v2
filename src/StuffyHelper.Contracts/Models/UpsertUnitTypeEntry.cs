using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class UpsertUnitTypeEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}
