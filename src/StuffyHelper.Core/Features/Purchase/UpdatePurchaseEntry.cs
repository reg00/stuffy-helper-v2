using StuffyHelper.Core.Features.PurchaseTag;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class UpdatePurchaseEntry
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }

        public List<PurchaseTagShortEntry> PurchaseTags { get; init; }
        [Required]
        public Guid UnitTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}
