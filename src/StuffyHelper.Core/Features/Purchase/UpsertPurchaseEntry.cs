using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class UpsertPurchaseEntry
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public Guid ShoppingId { get; set; }
        [Required]
        public List<string> PurchaseTags { get; init; }
        [Required]
        public Guid UnitTypeId { get; set; }
        public bool IsActive { get; set; }

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Cost = Cost,
                Amount = Amount,
                ShoppingId = ShoppingId,
                UnitTypeId = UnitTypeId,
                IsActive = true
            };
        }
    }
}
