namespace StuffyHelper.Minio.Features.Common
{
    /// <summary>
    /// Record for files
    /// </summary>
    public class MediaBlobEntry
    {
        /// <summary>
        /// Stream of file
        /// </summary>
        public Stream Stream { get; set; }

        /// <summary>
        /// Filename
        /// </summary>
        public string FileName { get; init; }
        
        /// <summary>
        /// Content type of file
        /// </summary>
        public string ContentType { get; init; }
        
        /// <summary>
        /// Extension of file
        /// </summary>
        public string Ext { get; init; }
    }
}
