using EnsureThat;
using StuffyHelper.Core.Features.Purchase;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class GetPurchaseTagEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public List<GetPurchaseEntry> Purchases { get; set; }

        public GetPurchaseTagEntry()
        {
            Purchases = new List<GetPurchaseEntry>();
        }

        public GetPurchaseTagEntry(PurchaseTagEntry entry, bool includePurchases)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
            Purchases = includePurchases ? entry.Purchases.Select(x => new GetPurchaseEntry(x, false, false, false)).ToList() : new List<GetPurchaseEntry>();
        }
    }
}
