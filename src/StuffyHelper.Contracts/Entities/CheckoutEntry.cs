namespace StuffyHelper.Contracts.Entities
{
    public class CheckoutEntry
    {
        public Guid Id { get; set; }

        public Guid EventId { get; init; }

        public DateTimeOffset CreatedDate { get; set; }

        public virtual EventEntry Event { get; set; }
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new List<PurchaseUsageEntry>();
        public virtual List<DebtEntry> Debts { get; set; } = new List<DebtEntry>();

        public CheckoutEntry()
        {
            
        }

        public CheckoutEntry(Guid eventId)
        {
            EventId = eventId;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
