using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Debt
{
    public class DebtEntry
    {
        public Guid Id { get; set; }
        public Guid EventId { get; set; }
        public string BorrowerId { get; set; }
        public string DebtorId { get; set; }
        public double Amount { get; set; }
        public double Paid { get; set; }
        public bool IsSent { get; set; }
        public bool IsComfirmed { get; set; }
        public DateTimeOffset CreatedDate { get; set; }

        public virtual EventEntry Event { get; set; }

        public DebtEntry()
        {
            IsSent = false;
            IsComfirmed = false;
            CreatedDate = DateTime.UtcNow;
            Paid = 0;
        }

        public DebtEntry(
            Guid eventId,
            string borrowerId,
            string debtorId,
            double amount)
        {
            EventId = eventId;
            BorrowerId = borrowerId;
            DebtorId = debtorId;
            Amount = amount;
            IsSent = false;
            IsComfirmed = false;
            CreatedDate = DateTime.UtcNow;
            Paid = 0;
        }
    }
}
