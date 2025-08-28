using System.ComponentModel.DataAnnotations;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Contracts.Models
{
    public class GetParticipantEntry
    {
        [Required]
        public Guid Id { get; init; }

        [Required]
        public GetUserEntry? User { get; init; }
        [Required]
        public EventShortEntry? Event { get; init; }
        public List<PurchaseShortEntry> Purchases { get; init; } = new();
        public IEnumerable<PurchaseUsageShortEntry> PurchaseUsages { get; init; } = new List<PurchaseUsageShortEntry>();
    }
}
