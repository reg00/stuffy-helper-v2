using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Models
{
    /// <summary>
    /// Model with detailed media information
    /// </summary>
    public class GetMediaEntry
    {
        /// <summary>
        /// Media id
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// Type of the file (extension)
        /// </summary>
        public FileType FileType { get; init; }
        
        /// <summary>
        /// Name of the file
        /// </summary>
        public string FileName { get; init; } = string.Empty;
        
        /// <summary>
        /// Type of media content
        /// </summary>
        public MediaType MediaType { get; init; }
        
        /// <summary>
        /// Link to access the media file
        /// </summary>
        public string Link { get; init; } = string.Empty;
        
        /// <summary>
        /// Event associated with the media
        /// </summary>
        public EventShortEntry? Event { get; init; }
    }
}