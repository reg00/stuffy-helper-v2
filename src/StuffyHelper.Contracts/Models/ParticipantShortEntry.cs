using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Contracts.Models
{
    public class ParticipantShortEntry
    {
        [Required]
        public Guid Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public Uri? ImageUri { get; init; }
    }
}
