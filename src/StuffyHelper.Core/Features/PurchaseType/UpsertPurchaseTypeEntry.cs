using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseType
{
    public class UpsertPurchaseTypeEntry
    {
        [Required]
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
