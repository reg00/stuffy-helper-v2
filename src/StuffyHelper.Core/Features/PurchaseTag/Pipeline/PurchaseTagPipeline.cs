using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Interfaces;
using StuffyHelper.Contracts.Models;

namespace StuffyHelper.Core.Features.PurchaseTag.Pipeline
{
    public class PurchaseTagPipeline : IPurchaseTagPipeline
    {
        private readonly IPurchaseTagStore _tagStore;

        public PurchaseTagPipeline(IPurchaseTagStore tagStore)
        {
            _tagStore = tagStore;
        }

        public async Task ProcessAsync(
            ITaggableEntry entry,
            IEnumerable<PurchaseTagShortEntry> tags,
            CancellationToken cancellationToken = default)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            if (tags is null || !tags.Any())
                return;

            entry.PurchaseTags.Clear();

            foreach (var tag in tags)
            {
                try
                {
                    var existsTag = await _tagStore.GetPurchaseTagAsync(tag.Name, cancellationToken);

                    entry.PurchaseTags.Add(existsTag);
                }
                catch (EntityNotFoundException)
                {
                    var newTag = await _tagStore.AddPurchaseTagAsync(new PurchaseTagEntry(tag.Name), cancellationToken);

                    entry.PurchaseTags.Add(newTag);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
    }
}
