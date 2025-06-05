using System.ComponentModel.DataAnnotations;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
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
