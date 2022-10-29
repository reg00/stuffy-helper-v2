using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Shopping
{
    public class AddShoppingEntry
    {
        [Required]
        public DateTime ShoppingDate { get; set; }
        [Required]
        public Guid ParticipantId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        [Required]
        public string Description { get; set; }

        public ShoppingEntry ToCommonEntry()
        {
            return new ShoppingEntry()
            {
                Check = null,
                ParticipantId = ParticipantId,
                EventId = EventId,
                ShoppingDate = ShoppingDate,
                Description = Description,
                IsActive = true
            };
        }
    }
}
