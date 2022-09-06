using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Shopping
{
    public class UpsertShoppingEntry
    {
        [Required]
        public DateTime ShoppingDate { get; set; }
        [Required]
        public Guid ParticipantId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        public string Check { get; set; }
        [Required]
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public ShoppingEntry ToCommonEntry()
        {
            return new ShoppingEntry()
            {
                Check = Check,
                ParticipantId = ParticipantId,
                EventId = EventId,
                ShoppingDate = ShoppingDate,
                Description = Description,
                IsActive = true
            };
        }
    }
}
