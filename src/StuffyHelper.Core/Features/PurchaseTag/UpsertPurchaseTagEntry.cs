using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class UpsertPurchaseTagEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;

        public PurchaseTagEntry ToCommonEntry()
        {
            return new PurchaseTagEntry()
            {
                Name = Name,
                IsActive = true
            };
        }
    }
}
