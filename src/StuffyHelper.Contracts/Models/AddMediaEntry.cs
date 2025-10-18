using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using StuffyHelper.Contracts.Enums;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model for creating media
    /// </summary>
    public class AddMediaEntry
    {
        ///// <summary>
        ///// Event id
        ///// </summary>
        //[Required]
        //public Guid EventId { get; init; }
        
        /// <summary>
        /// File
        /// Optional fill file or link
        /// </summary>
        public IFormFile? File { get; set; }
        
        /// <summary>
        /// Media type
        /// </summary>
        [Required]
        public MediaType MediaType { get; init; }
        
        /// <summary>
        /// Link
        /// Optional fill file or link
        /// </summary>
        public string Link { get; init; } = string.Empty;
    }
}
