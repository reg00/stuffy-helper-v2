using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    public class UnitTypeEntry
    {
        public Guid Id { get; init; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }

        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();

        public void PatchFrom(UpsertUnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
        }
    }
}
