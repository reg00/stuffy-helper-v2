using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class PurchaseShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }

        public PurchaseShortEntry(PurchaseEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Cost = entry.Cost;
            Weight = entry.Weight;
            Count = entry.Count;
        }
    }
}
