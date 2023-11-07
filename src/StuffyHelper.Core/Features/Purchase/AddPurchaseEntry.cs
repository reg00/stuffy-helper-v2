using StuffyHelper.Core.Features.PurchaseTag;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Purchase
{
    public class AddPurchaseEntry
    {
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public double Cost { get; set; }
        [Required]
        public double Amount { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public Guid ParticipantId { get; set; }
        public List<PurchaseTagShortEntry> PurchaseTags { get; init; } = new List<PurchaseTagShortEntry>();
        [Required]
        public Guid UnitTypeId { get; set; }
        [Required]
        public bool IsPartial { get; set; }

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
