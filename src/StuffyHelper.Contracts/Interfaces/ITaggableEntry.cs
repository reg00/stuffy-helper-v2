using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Interfaces
{
    /// <summary>
    /// Taggable entry interface
    /// </summary>
    public interface ITaggableEntry
    {
        List<PurchaseTagEntry> PurchaseTags { get; set; }
    }
}
