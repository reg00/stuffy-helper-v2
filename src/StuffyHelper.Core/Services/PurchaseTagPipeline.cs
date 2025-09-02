using EnsureThat;
using Reg00.Infrastructure.Errors;
using StuffyHelper.Contracts.Entities;
using StuffyHelper.Contracts.Interfaces;
using StuffyHelper.Contracts.Models;
using StuffyHelper.Core.Services.Interfaces;
using StuffyHelper.Data.Repository.Interfaces;

namespace StuffyHelper.Core.Services
{
    public class PurchaseTagPipeline : IPurchaseTagPipeline
    {
        /// <inheritdoc />
        private readonly IPurchaseTagRepository _tagRepository;

        /// <summary>
        /// Ctor.
        /// </summary>
        public PurchaseTagPipeline(IPurchaseTagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        /// <inheritdoc />
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
                    var existsTag = await _tagRepository.GetPurchaseTagAsync(tag.Name, cancellationToken);

                    entry.PurchaseTags.Add(existsTag);
                }
                catch (EntityNotFoundException)
                {
                    var newTag = await _tagRepository.AddPurchaseTagAsync(new PurchaseTagEntry(tag.Name), cancellationToken);

                    entry.PurchaseTags.Add(newTag);
                }
            }
        }
    }
}
