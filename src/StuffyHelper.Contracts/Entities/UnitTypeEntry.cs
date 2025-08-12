using EnsureThat;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Unit type entity
    /// </summary>
    public class UnitTypeEntry
    {
        /// <summary>
        /// Identifier of unit type
        /// </summary>
        public Guid Id { get; init; }
        /// <summary>
        /// Unit type name
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

        public void PatchFrom(UpsertUnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Name = entry.Name;
        }
    }
}
