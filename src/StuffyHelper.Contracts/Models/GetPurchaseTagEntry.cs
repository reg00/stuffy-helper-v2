using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class GetPurchaseTagEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;

        public List<PurchaseShortEntry> Purchases { get; init; }
    }
}
