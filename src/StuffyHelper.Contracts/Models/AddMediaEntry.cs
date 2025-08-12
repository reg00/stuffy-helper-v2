using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Contracts.Enums;

namespace StuffyHelper.Contracts.Models
{
    public class AddMediaEntry
    {
        [Required]
        public Guid EventId { get; init; }
        public IFormFile? File { get; set; }
        [Required]
        public MediaType MediaType { get; init; }
        public string Link { get; init; } = string.Empty;
    }
}
