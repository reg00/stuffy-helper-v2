using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class UpsertPurchaseTagEntry
    {
        [Required]
        public string Name { get; set; }
        public bool IsActive { get; set; }

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
