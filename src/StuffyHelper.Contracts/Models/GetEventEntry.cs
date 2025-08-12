using System.ComponentModel.DataAnnotations;
using StuffyHelper.Authorization.Contracts.Models;

namespace StuffyHelper.Contracts.Models
{
    public class GetEventEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        [Required]
        public DateTime CreatedDate { get; init; }
        [Required]
        public DateTime EventDateStart { get; init; }
        public DateTime? EventDateEnd { get; init; }
        [Required]
        public bool IsCompleted { get; init; }
        public Uri? MediaUri { get; init; }


        [Required]
        public UserShortEntry? User { get; init; }
        public List<ParticipantShortEntry> Participants { get; init; } = new();
        public List<PurchaseShortEntry> Purchases { get; init; } = new();
        public List<MediaShortEntry> Medias { get; init; } = new();
    }
}
