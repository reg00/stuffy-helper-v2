namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Checkout entity
    /// </summary>
    public class CheckoutEntry
    {
        /// <summary>
        /// Identifier of checkout
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; init; }

        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Linked event entity
        /// </summary>
        public virtual EventEntry Event { get; set; }
        
        /// <summary>
        /// Linked purchases
        /// </summary>
        public virtual List<PurchaseUsageEntry> PurchaseUsages { get; set; } = new();
        
        /// <summary>
        /// Linked debts
        /// </summary>
        public virtual List<DebtEntry> Debts { get; set; } = new();

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
