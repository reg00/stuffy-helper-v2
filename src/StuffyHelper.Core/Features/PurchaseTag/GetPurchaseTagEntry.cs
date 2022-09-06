using EnsureThat;
using StuffyHelper.Core.Features.Purchase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StuffyHelper.Core.Features.PurchaseTag
{
    public class GetPurchaseTagEntry
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
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
            IsActive = entry.IsActive;
            Purchases = includePurchases ? entry.Purchases.Select(x => new GetPurchaseEntry(x, false, false, false)).ToList() : new List<GetPurchaseEntry>();
        }
    }
}
