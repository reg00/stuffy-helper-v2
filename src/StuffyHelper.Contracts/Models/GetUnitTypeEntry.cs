using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class GetUnitTypeEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;

        public List<PurchaseShortEntry> Purchases { get; init; }

        public GetUnitTypeEntry(UnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Purchases = entry.Purchases.Select(x => new PurchaseShortEntry(x)).ToList();
        }
    }
}
