namespace StuffyHelper.Core.Features.Purchase
{
    public class UpsertPurchaseEntry
    {
        public string Name { get; set; }
        public double Cost { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public Guid ShoppingId { get; set; }
        public Guid PurchaseTypeId { get; set; }
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
