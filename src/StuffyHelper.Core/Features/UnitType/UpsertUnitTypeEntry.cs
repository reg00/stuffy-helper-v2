namespace StuffyHelper.Core.Features.UnitType
{
    public class UpsertUnitTypeEntry
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

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
