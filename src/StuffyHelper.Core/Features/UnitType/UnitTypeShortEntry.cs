using EnsureThat;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.UnitType
{
    public class UnitTypeShortEntry
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public UnitTypeShortEntry(UnitTypeEntry entry)
        {
            EnsureArg.IsNotNull(entry, nameof(entry));

            Id = entry.Id;
            Name = entry.Name;
        }
    }
}
