using StuffyHelper.Core.Features.PurchaseTag;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Core.Features.Purchase
{
    public class UpdatePurchaseEntry
    {
        [Required]
        public string Name { get; init; } = string.Empty;
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Cost { get; init; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Amount { get; init; }
        [Required]
        public bool IsPartial { get; init; }

        public List<PurchaseTagShortEntry> PurchaseTags { get; init; } = new List<PurchaseTagShortEntry>();
        [Required]
        public Guid UnitTypeId { get; init; }
    }
}
