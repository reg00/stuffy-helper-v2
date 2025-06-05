using System.ComponentModel.DataAnnotations;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class UpsertUnitTypeEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        public UnitTypeEntry ToCommonEntry()
        {
            return new UnitTypeEntry()
            {
                Name = Name,
                IsActive = true
            };
        }
    }
}
