using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.UnitType
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
