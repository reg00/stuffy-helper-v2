namespace StuffyHelper.Core.Features.Purchase
{
    public class UpsertPurchaseEntry
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
        public Guid ShoppingId { get; set; }
        public Guid PurchaseTypeId { get; set; }
        public bool IsActive { get; set; }

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Amount = Amount,
                Weight = Weight,
                ShoppingId = ShoppingId,
                PurchaseTypeId = PurchaseTypeId,
                Count = Count,
                IsActive = true
            };
        }
    }
}
