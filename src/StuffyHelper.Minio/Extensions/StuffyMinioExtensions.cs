using System.Text;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Minio.Extensions
{
    /// <summary>
    /// Minio extensions
    /// </summary>
    public static class StuffyMinioExtensions
    {
        /// <summary>
        /// Get object name by event and file id
        /// </summary>
        public static string GetStuffyObjectName(
            string eventId,
            string fileId,
            FileType fileType)
        {
            var sb = new StringBuilder();

            sb.Append($"data/{eventId}/");
            sb.Append($"{fileId}.{fileType.ToString().ToLowerInvariant()}");

            return sb.ToString().TrimEnd('/');
        }
    }
}
