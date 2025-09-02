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
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Cost { get; init; }
        
        /// <summary>
        /// Amount
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Значение должно быть больше 0")]
        public double Amount { get; init; }
        
        /// <summary>
        /// Event id
        /// </summary>
        [Required]
        public Guid EventId { get; init; }
        
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
