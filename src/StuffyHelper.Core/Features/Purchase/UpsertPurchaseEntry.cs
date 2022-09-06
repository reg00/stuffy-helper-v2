using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class UpsertPurchaseEntry
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double Cost { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        [Required]
        public Guid ShoppingId { get; set; }
        [Required]
        public Guid PurchaseTypeId { get; set; }
        [Required]
        public Guid UnitTypeId { get; set; }
        public bool IsActive { get; set; }

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Cost = Cost,
                Weight = Weight,
                ShoppingId = ShoppingId,
                PurchaseTypeId = PurchaseTypeId,
                UnitTypeId = UnitTypeId,
                Count = Count,
                IsActive = true
            };
        }
    }
}
