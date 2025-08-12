using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Contracts.Models
{
    public class AddPurchaseEntry
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
        public Guid EventId { get; init; }
        [Required]
        public Guid ParticipantId { get; init; }
        public List<PurchaseTagShortEntry> PurchaseTags { get; set; } = new List<PurchaseTagShortEntry>();
        [Required]
        public Guid UnitTypeId { get; init; }
        [Required]
        public bool IsPartial { get; init; }
    }
}
