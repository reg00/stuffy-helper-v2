using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace StuffyHelper.Core.Features.Event
{
    public class AddEventEntry
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public DateTime EventDateStart { get; set; }
        public IFormFile? File { get; set; }
        public string? Description { get; set; }
        public DateTime? EventDateEnd { get; set; }
    }
}
