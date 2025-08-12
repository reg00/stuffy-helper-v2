using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Authorization.Contracts.Models;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class GetPurchaseUsageEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public ParticipantShortEntry? Participant { get; init; }
        [Required]
        public PurchaseShortEntry? Purchase { get; init; }
        public double Amount { get; init; }
    }
}
