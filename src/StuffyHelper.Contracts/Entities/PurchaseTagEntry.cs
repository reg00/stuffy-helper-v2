using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Purchase tag entity
    /// </summary>
    public class PurchaseTagEntry
    {
        /// <summary>
        /// Identifier of purchase tag
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Is active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Linked purchases
        /// </summary>
        public virtual List<PurchaseEntry> Purchases { get; set; } = new();

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
