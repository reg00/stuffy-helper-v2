using EnsureThat;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.UnitType
{
    public class UnitTypeEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();

        public void PatchFrom(UpsertUnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
        }
    }
}
