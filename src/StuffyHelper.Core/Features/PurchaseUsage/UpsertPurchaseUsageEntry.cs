namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class UpsertPurchaseUsageEntry
    {
        public Guid PurchaseId { get; set; }
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
