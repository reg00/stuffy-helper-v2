using EnsureThat;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.UnitType
{
    public class GetUnitTypeEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public List<GetPurchaseEntry> Purchases { get; set; }

        public GetUnitTypeEntry()
        {
            Purchases = new List<GetPurchaseEntry>();
        }

        public GetUnitTypeEntry(UnitTypeEntry entry, bool includePurchases)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            IsActive = entry.IsActive;
            Purchases = includePurchases ? entry.Purchases.Select(x => new GetPurchaseEntry(x, false, false, false, false)).ToList() : new List<GetPurchaseEntry>();
        }
    }
}
