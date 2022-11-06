using EnsureThat;
using StuffyHelper.Core.Features.Purchase;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class PurchaseTagEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public virtual List<PurchaseEntry> Purchases { get; set; } = new List<PurchaseEntry>();

        public PurchaseTagEntry()
        {

        }

        public PurchaseTagEntry(string name)
        {
            Name = name;
            IsActive = true;

            Purchases = new List<PurchaseEntry>();
        }

        public void PatchFrom(UpsertPurchaseTagEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
        }
    }
}
