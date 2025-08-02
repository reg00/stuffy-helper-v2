namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Debt entity
    /// </summary>
    public class DebtEntry
    {
        /// <summary>
        /// Identifier of debt
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; init; }
        
        /// <summary>
        /// Checkout id
        /// </summary>
        public Guid? CheckoutId { get; set; }
        
        /// <summary>
        /// Lender id
        /// </summary>
        public string LenderId { get; set; } = string.Empty;
        
        /// <summary>
        /// Debtor id
        /// </summary>
        public string DebtorId { get; set; } = string.Empty;
        
        /// <summary>
        /// Amount of debt
        /// </summary>
        public double Amount { get; set; }
        
        /// <summary>
        /// Is sent
        /// </summary>
        public bool IsSent { get; set; }
        
        /// <summary>
        /// Is confirmed
        /// </summary>
        public bool IsComfirmed { get; set; }
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }

        /// <summary>
        /// Linked event
        /// </summary>
        public virtual EventEntry Event { get; init; }
        /// <summary>
        /// Linked checkout
        /// </summary>
        public virtual CheckoutEntry Checkout { get; set; }

        public DebtEntry()
        {
            IsSent = false;
            IsComfirmed = false;
            CreatedDate = DateTime.UtcNow;
        }

        public DebtEntry(
            Guid eventId,
            string lenderId,
            string debtorId,
            double amount)
        {
            EventId = eventId;
            LenderId = lenderId;
            DebtorId = debtorId;
            Amount = amount;
            IsSent = false;
            IsComfirmed = false;
            CreatedDate = DateTime.UtcNow;
        }
    }
}
