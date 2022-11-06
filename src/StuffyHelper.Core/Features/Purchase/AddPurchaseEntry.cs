using StuffyHelper.Core.Features.PurchaseTag;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class AddPurchaseEntry
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public Guid ShoppingId { get; set; }
        public List<PurchaseTagShortEntry> PurchaseTags { get; init; }
        [Required]
        public Guid UnitTypeId { get; set; }

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Cost = Cost,
                Amount = Amount,
                ShoppingId = ShoppingId,
                UnitTypeId = UnitTypeId
            };
        }
    }
}
