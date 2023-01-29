using EnsureThat;
using StuffyHelper.Authorization.Core.Models;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseUsage
{
    public class PurchaseUsageShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? PurchaseName { get; set; }

        public PurchaseUsageShortEntry(PurchaseUsageEntry entry, UserEntry? user = null)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            UserId = entry.Participant?.UserId;
            PurchaseName = entry?.Purchase?.Name;
            Name = user?.Name;
        }
    }
}
