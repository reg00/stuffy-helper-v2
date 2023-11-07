namespace StuffyHelper.Minio.Features.Common
{
    public static class FileTypeMapper
    {
        private static readonly Dictionary<string, FileType> FileTypeExtMap = new Dictionary<string, FileType>()
        {
            { KnownFileTypes.Jpeg, FileType.Jpeg },
            { KnownFileTypes.Jpg, FileType.Jpg },
            { KnownFileTypes.Png, FileType.Png },
            { KnownFileTypes.Pdf, FileType.Pdf },
            { KnownFileTypes.Txt, FileType.Txt },
            { KnownFileTypes.Doc, FileType.Doc },
            { KnownFileTypes.Docx, FileType.Docx },
            { KnownFileTypes.Xls, FileType.Xls },
            { KnownFileTypes.Xlsx, FileType.Xlsx }
        };

        private static readonly Dictionary<FileType, string> FileTypeContentTypeMap = new Dictionary<FileType, string>()
        {
            { FileType.Jpg, KnownMinioContentTypes.ImageJpeg },
            { FileType.Jpeg, KnownMinioContentTypes.ImageJpeg },
            { FileType.Pdf, KnownMinioContentTypes.ApplicationPdf },
            { FileType.Png, KnownMinioContentTypes.ImagePng },
            { FileType.Txt, KnownMinioContentTypes.TextPlain },
            { FileType.Doc, KnownMinioContentTypes.ApplicationDoc },
            { FileType.Docx, KnownMinioContentTypes.ApplicationDocx },
            { FileType.Xls, KnownMinioContentTypes.ApplicationXls },
            { FileType.Xlsx, KnownMinioContentTypes.ApplicationXlsx }
        };

        private static readonly Dictionary<string, string> ContentTypeExtMap = new Dictionary<string, string>()
        {
            { KnownFileTypes.Jpg, KnownMinioContentTypes.ImageJpeg },
            { KnownFileTypes.Jpeg, KnownMinioContentTypes.ImageJpeg },
            { KnownFileTypes.Png, KnownMinioContentTypes.ImagePng },
            { KnownFileTypes.Pdf, KnownMinioContentTypes.ApplicationPdf },
            { KnownFileTypes.Txt, KnownMinioContentTypes.TextPlain },
            { KnownFileTypes.Doc, KnownMinioContentTypes.ApplicationDoc },
            { KnownFileTypes.Docx, KnownMinioContentTypes.ApplicationDocx },
            { KnownFileTypes.Xls, KnownMinioContentTypes.ApplicationXls },
            { KnownFileTypes.Xlsx, KnownMinioContentTypes.ApplicationXlsx }
        };

        private static readonly Dictionary<FileType, string> ExtFileTypeMap = new Dictionary<FileType, string>()
        {
            { FileType.Jpg, KnownFileTypes.Jpg },
            { FileType.Jpeg, KnownFileTypes.Jpeg },
            { FileType.Png, KnownFileTypes.Png },
            { FileType.Pdf, KnownFileTypes.Pdf },
            { FileType.Txt, KnownFileTypes.Txt },
            { FileType.Doc, KnownFileTypes.Doc },
            { FileType.Docx, KnownFileTypes.Docx },
            { FileType.Xls, KnownFileTypes.Xls },
            { FileType.Xlsx, KnownFileTypes.Xlsx }
        };

        public static FileType MapFileTypeFromExt(string ext)
        {
            if (FileTypeExtMap.TryGetValue(ext, out FileType result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"File type {ext} is not supported.");
            }
        }

        public static string MapContentTypeFromFileType(FileType fileType)
        {
            if (FileTypeContentTypeMap.TryGetValue(fileType, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"File type {fileType} is not supported.");
            }
        }

        public static string MapContentTypeFromExt(string ext)
        {
            if (ContentTypeExtMap.TryGetValue(ext, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"File type {ext} is not supported.");
            }
        }

        public static string MapExtFromFileType(FileType fileType)
        {
            if (ExtFileTypeMap.TryGetValue(fileType, out var result))
            {
                return result;
            }
            else
            {
                throw new NotSupportedException($"File type {fileType} is not supported.");
            }
        }

        public static bool ValidateExtIsImage(string ext)
        {
            if (FileTypeExtMap.TryGetValue(ext, out FileType result))
            {
                if (result == FileType.Jpeg || result == FileType.Jpg || result == FileType.Png)
                    return true;

                throw new NotSupportedException($"File must be an image.");
            }
            else
            {
                throw new NotSupportedException($"File type {ext} is not supported.");
            }
        }
    }
}
