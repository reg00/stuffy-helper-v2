using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Shopping
{
    public class ShoppingShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public DateTime ShoppingDate { get; set; }
        public string Check { get; set; }
        [Required]
        public string Description { get; set; }

        public ShoppingShortEntry(ShoppingEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            ShoppingDate = entry.ShoppingDate;
            Check = entry.Check;
            Description = entry.Description;
        }
    }
}
