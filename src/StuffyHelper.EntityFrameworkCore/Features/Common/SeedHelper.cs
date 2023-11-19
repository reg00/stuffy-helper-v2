using StuffyHelper.Core.Features.UnitType;

namespace StuffyHelper.EntityFrameworkCore.Features.Common
{
    public static class SeedHelper
    {
        public static IList<UnitTypeEntry> GetSeedUnitTypes()
        {
            return new List<UnitTypeEntry>()
            {
                new()
                {
                    Id = Guid.Parse("7142d1aa-53b1-416c-80b8-18b5d3ba33ab"),
                    IsActive = true,
                    Name = "Шт."
                },
                new()
                {
                    Id = Guid.Parse("32939f8e-3818-4753-8d1b-4ba8ab7783f9"),
                    IsActive = true,
                    Name = "Кг."
                },
                new()
                {
                    Id = Guid.Parse("f73043eb-9e20-4934-845a-7722557f164e"),
                    IsActive = true,
                    Name = "Гр."
                },
                new()
                {
                    Id = Guid.Parse("6700cac9-f36e-4697-a6fe-27fdbaebd267"),
                    IsActive = true,
                    Name = "Л."
                },
                new()
                {
                    Id = Guid.Parse("320053b6-110a-4358-9289-21e64d718b60"),
                    IsActive = true,
                    Name = "Мл."
                },
                new()
                {
                    Id = Guid.Parse("eda2a0fe-539c-471d-9941-e0ce8982e923"),
                    IsActive = true,
                    Name = "Уп."
                }
            };
        }
    }
}
