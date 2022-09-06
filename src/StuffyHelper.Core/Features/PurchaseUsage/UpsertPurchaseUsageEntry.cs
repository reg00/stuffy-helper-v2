using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class UpsertPurchaseUsageEntry
    {
        [Required]
        public Guid PurchaseId { get; set; }
        [Required]
        public Guid ParticipantId { get; set; }

        public PurchaseUsageEntry ToCommonEntry()
        {
            return new PurchaseUsageEntry()
            {
                PurchaseId = PurchaseId,
                ParticipantId = ParticipantId
            };
        }
    }
}
