namespace StuffyHelper.Core.Features.Purchase
{
    public class UpsertPurchaseEntry
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int Count { get; set; }
        public Guid ShoppingId { get; set; }

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Amount = Amount,
                ShoppingId = ShoppingId,
                Count = Count
            };
        }
    }
}
