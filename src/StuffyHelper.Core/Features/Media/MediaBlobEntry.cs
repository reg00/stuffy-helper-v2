using EnsureThat;
using StuffyHelper.Core.Features.Common;

namespace StuffyHelper.Core.Features.Media
{
    public class MediaBlobEntry
    {
        public MediaBlobEntry(Stream stream, FileType fileType)
        {
            EnsureArg.IsNotNull(stream, nameof(stream));
            stream.Seek(0, SeekOrigin.Begin);

            Stream = stream;
            ContentType = FileTypeMapper.MapContentTypeFromFileType(fileType);
            Ext = FileTypeMapper.MapExtFromFileType(fileType);
        }

        public Stream Stream { get; set; }

        public string ContentType { get; set; }

        public string Ext { get; set; }
    }
}
