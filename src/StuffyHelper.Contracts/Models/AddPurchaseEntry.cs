using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for create new purchase
    /// </summary>
    public class AddPurchaseEntry
    {
        /// <summary>
        /// Name
        /// </summary>
        [Required]
        public string Name { get; init; } = string.Empty;
        
        /// <summary>
        /// Cost
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Cost { get; init; }
        
        /// <summary>
        /// Amount
        /// </summary>
        [Required]
        [Range(1, long.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public long Amount { get; init; }
        
        /// <summary>
        /// Participant id
        /// </summary>
        [Required]
        public Guid ParticipantId { get; init; }
        
        /// <summary>
        /// Purchase tags
        /// </summary>
        public List<PurchaseTagShortEntry> PurchaseTags { get; set; } = new();
        
        /// <summary>
        /// Unit type id
        /// </summary>
        [Required]
        public Guid UnitTypeId { get; init; }
        
        /// <summary>
        /// Is purchase partial
        /// </summary>
        [Required]
        public bool IsPartial { get; init; }
    }
}
