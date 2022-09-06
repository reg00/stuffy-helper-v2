namespace StuffyHelper.Core.Features.PurchaseTag.Pipeline
{
    public interface IPurchaseTagPipeline
    {
        Task ProcessAsync(
            ITaggableEntry entry,
            IEnumerable<string> tags,
            CancellationToken cancellationToken = default);
    }
}
