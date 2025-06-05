namespace StuffyHelper.Contracts.Entities
{
    public class DebtEntry
    {
        public Guid Id { get; init; }
        public Guid EventId { get; init; }
        public Guid? CheckoutId { get; set; }
        public string LenderId { get; set; } = string.Empty;
        public string DebtorId { get; set; } = string.Empty;
        public double Amount { get; set; }
        public bool IsSent { get; set; }
        public bool IsComfirmed { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual EventEntry Event { get; init; }
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
