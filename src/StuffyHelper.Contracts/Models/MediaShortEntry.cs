using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with short media information
    /// </summary>
    public class MediaShortEntry
    {
        /// <summary>
        /// Media id
        /// </summary>
        public Guid Id { get; set; }
        
        /// <summary>
        /// Type of the file (extension)
        /// </summary>
        public FileType FileType { get; set; }
        
        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; set; } = string.Empty;
        
        /// <summary>
        /// Type of media content
        /// </summary>
        public MediaType MediaType { get; set; }
        
        /// <summary>
        /// Link to access the media file
        /// </summary>
        public string Link { get; set; } = string.Empty;
    }
}