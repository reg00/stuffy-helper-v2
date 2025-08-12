using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class EventShortEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        [Required]
        public DateTime? EventDateStart { get; init; }
        public DateTime? EventDateEnd { get; init;}
        [Required]
        public bool IsCompleted { get; init; }
        public Uri? ImageUri { get; init; }
    }
}
