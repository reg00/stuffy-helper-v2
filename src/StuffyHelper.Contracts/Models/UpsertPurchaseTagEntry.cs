using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class UpsertPurchaseTagEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;
    }
}
