using EnsureThat;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.UnitType
{
    public class GetUnitTypeEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public List<PurchaseShortEntry> Purchases { get; set; }

        public GetUnitTypeEntry(UnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Purchases = entry.Purchases.Select(x => new PurchaseShortEntry(x)).ToList();
        }
    }
}
