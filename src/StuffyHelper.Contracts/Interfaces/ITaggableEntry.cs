using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Interfaces
{
    public interface ITaggableEntry
    {
        List<PurchaseTagEntry> PurchaseTags { get; set; }
    }
}
