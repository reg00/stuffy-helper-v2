using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Core.Features.Event;

namespace StuffyHelper.Core.Features.Debt
{
    public class GetDebtEntry
    {
        public Guid Id { get; set; }
        public double Amount { get; set; }
        public bool IsSent { get; set; }
        public bool IsComfirmed { get; set; }

        public EventShortEntry Event { get; set; }
        public UserShortEntry Lender { get; set; }
        public UserShortEntry Debtor { get; set; }

        public GetDebtEntry(
            DebtEntry entry,
            UserShortEntry lender,
            UserShortEntry debtor)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));
            EnsureArg.IsNotNull(lender, nameof(lender));
            EnsureArg.IsNotNull(debtor, nameof(debtor));

            Id = entry.Id;
            Amount = entry.Amount;
            IsSent = entry.IsSent;
            IsComfirmed = entry.IsComfirmed;
            Event = new EventShortEntry(entry.Event);
            Lender = lender;
            Debtor = debtor;
        }
    }
}
