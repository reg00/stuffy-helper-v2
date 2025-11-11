using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed debt description
    /// </summary>
    public class GetDebtEntry
    {
        /// <summary>
        /// Debt id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Amount
        /// </summary>
        public long Amount { get; set; }
        
        /// <summary>
        /// Is debt paid
        /// </summary>
        public bool IsSent { get; set; }
        
        /// <summary>
        /// Is payment confirmed
        /// </summary>
        public bool IsComfirmed { get; set; }

        /// <summary>
        /// Event link
        /// </summary>
        public EventShortEntry Event { get; set; }
        
        /// <summary>
        /// Lender user link
        /// </summary>
        public UserShortEntry Lender { get; set; }
        
        /// <summary>
        /// Debtor user link
        /// </summary>
        public UserShortEntry Debtor { get; set; }
    }
}
