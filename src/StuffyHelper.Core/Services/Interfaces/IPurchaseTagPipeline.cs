using StuffyHelper.Contracts.Interfaces;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Services.Interfaces
{
    public interface IPurchaseTagPipeline
    {
        Task ProcessAsync(
            ITaggableEntry entry,
            IEnumerable<PurchaseTagShortEntry> tags,
            CancellationToken cancellationToken = default);
    }
}
