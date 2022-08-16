namespace StuffyHelper.Core.Features.PurchaseType
{
    public class UpsertPurchaseTypeEntry
    {
        public string Name { get; set; }
        public bool IsActive { get; set; }

        public PurchaseTypeEntry ToCommonEntry()
        {
            return new PurchaseTypeEntry()
            {
                Name = Name,
                IsActive = true
            };
        }
    }
}
