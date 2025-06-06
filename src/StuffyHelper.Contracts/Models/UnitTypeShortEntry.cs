using System.ComponentModel.DataAnnotations;
using EnsureThat;
using StuffyHelper.Contracts.Entities;

namespace StuffyHelper.Contracts.Models
{
    public class UnitTypeShortEntry
    {
        [Required]
        public Guid Id { get; init; }
        [Required]
        public string Name { get; init; } = string.Empty;

        public UnitTypeShortEntry(UnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
        }
    }
}
