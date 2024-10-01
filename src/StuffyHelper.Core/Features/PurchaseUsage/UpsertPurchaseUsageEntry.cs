using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class UpsertPurchaseUsageEntry
    {
        [Required]
        public Guid PurchaseId { get; init; }
        [Required]
        public Guid ParticipantId { get; init; }
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Amount { get; init; }

        public PurchaseUsageEntry ToCommonEntry()
        {
            return new PurchaseUsageEntry()
            {
                PurchaseId = PurchaseId,
                ParticipantId = ParticipantId,
                Amount = Amount,
            };
        }
    }
}
