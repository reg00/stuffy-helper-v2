namespace StuffyHelper.Core.Features.PurchaseTag
{
    public interface ITaggableEntry
    {
        List<PurchaseTagEntry> PurchaseTags { get; set; }
    }
}
