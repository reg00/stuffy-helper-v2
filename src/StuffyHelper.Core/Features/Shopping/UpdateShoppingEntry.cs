using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Shopping
{
    public class UpdateShoppingEntry
    {
        [Required]
        public DateTime ShoppingDate { get; set; }
        [Required]
        public Guid ParticipantId { get; set; }
        public string? Check { get; set; }
        [Required]
        public string Description { get; set; }
    }
}
