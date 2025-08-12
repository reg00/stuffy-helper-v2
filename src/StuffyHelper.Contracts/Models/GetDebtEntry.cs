using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
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
    }
}
