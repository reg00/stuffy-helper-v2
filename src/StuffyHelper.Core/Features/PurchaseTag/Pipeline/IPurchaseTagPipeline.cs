namespace StuffyHelper.Core.Features.PurchaseTag.Pipeline
{
    public interface IPurchaseTagPipeline
    {
        Task ProcessAsync(
            ITaggableEntry entry,
            IEnumerable<PurchaseTagShortEntry> tags,
            CancellationToken cancellationToken = default);
    }
}
