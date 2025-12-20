using StuffyHelper.Contracts.Enums;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Contracts.Entities
{
    /// <summary>
    /// Media entity
    /// </summary>
    public class MediaEntry
    {
        /// <summary>
        /// Identifier of media
        /// </summary>
        public Guid Id { get; init; }
        
        /// <summary>
        /// Event id
        /// </summary>
        public Guid EventId { get; set; }
        
        /// <summary>
        /// File name
        /// </summary>
        public string FileName { get; init; } = string.Empty;
        
        /// <summary>
        /// Created date
        /// </summary>
        public DateTimeOffset CreatedDate { get; set; }
        
        /// <summary>
        /// File type
        /// </summary>
        public FileType FileType { get; init; }
        
        /// <summary>
        /// Media type
        /// </summary>
        public MediaType MediaType { get; init; }
        
        /// <summary>
        /// Link
        /// </summary>
        public string Link { get; set; } = string.Empty;
        
        /// <summary>
        /// Is media primal
        /// </summary>
        public bool IsPrimal { get; init; }

        /// <summary>
        /// Linked event
        /// </summary>
        public virtual EventEntry Event { get; init; }
    }
}
