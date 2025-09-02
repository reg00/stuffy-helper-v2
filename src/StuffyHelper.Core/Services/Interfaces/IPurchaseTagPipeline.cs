using StuffyHelper.Contracts.Interfaces;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    /// <summary>
    /// Interface for processing purchase tags pipeline
    /// </summary>
    public interface IPurchaseTagPipeline
    {
        /// <summary>
        /// Process tags for a taggable entry
        /// </summary>
        /// <param name="entry">Taggable entry to process tags for</param>
        /// <param name="tags">Tags to associate with the entry</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task ProcessAsync(
            ITaggableEntry entry,
            IEnumerable<PurchaseTagShortEntry> tags,
            CancellationToken cancellationToken = default);
    }
}