using System.Text;
using StuffyHelper.Minio.Features.Common;

namespace StuffyHelper.Core.Extensions
{
    public static class StuffyMinioExtensions
    {
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
