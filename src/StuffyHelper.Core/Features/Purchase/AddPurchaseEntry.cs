using StuffyHelper.Core.Features.PurchaseTag;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Features.Purchase
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

        public PurchaseEntry ToCommonEntry()
        {
            return new PurchaseEntry()
            {
                Name = Name,
                Cost = Cost,
                Amount = Amount,
                EventId = EventId,
                UnitTypeId = UnitTypeId,
                IsPartial = IsPartial,
                ParticipantId = ParticipantId,
                CreatedDate = DateTime.UtcNow,
            };
        }
    }
}
