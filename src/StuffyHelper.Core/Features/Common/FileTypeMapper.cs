using StuffyHelper.Core.Exceptions;
using StuffyHelper.Core.Features.Media;
using StuffyHelper.Core.Web;

namespace StuffyHelper.Core.Features.Common
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
            { FileType.Jpg, KnownContentTypes.ImageJpeg },
            { FileType.Jpeg, KnownContentTypes.ImageJpeg },
            { FileType.Pdf, KnownContentTypes.ApplicationPdf },
            { FileType.Png, KnownContentTypes.ImagePng },
            { FileType.Txt, KnownContentTypes.TextPlain },
            { FileType.Doc, KnownContentTypes.ApplicationDoc },
            { FileType.Docx, KnownContentTypes.ApplicationDocx },
            { FileType.Xls, KnownContentTypes.ApplicationXls },
            { FileType.Xlsx, KnownContentTypes.ApplicationXlsx }
        };

        private static readonly Dictionary<string, string> ContentTypeExtMap = new Dictionary<string, string>()
        {
            { KnownFileTypes.Jpg, KnownContentTypes.ImageJpeg },
            { KnownFileTypes.Jpeg, KnownContentTypes.ImageJpeg },
            { KnownFileTypes.Png, KnownContentTypes.ImagePng },
            { KnownFileTypes.Pdf, KnownContentTypes.ApplicationPdf },
            { KnownFileTypes.Txt, KnownContentTypes.TextPlain },
            { KnownFileTypes.Doc, KnownContentTypes.ApplicationDoc },
            { KnownFileTypes.Docx, KnownContentTypes.ApplicationDocx },
            { KnownFileTypes.Xls, KnownContentTypes.ApplicationXls },
            { KnownFileTypes.Xlsx, KnownContentTypes.ApplicationXlsx }
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
                throw new FileTypeNotSupportedException($"File type {ext} is not supported.");
            }
        }

        public static string MapContentTypeFromFileType(FileType fileType)
        {
            if (FileTypeContentTypeMap.TryGetValue(fileType, out string result))
            {
                return result;
            }
            else
            {
                throw new FileTypeNotSupportedException($"File type {fileType} is not supported.");
            }
        }

        public static string MapContentTypeFromExt(string ext)
        {
            if (ContentTypeExtMap.TryGetValue(ext, out string result))
            {
                return result;
            }
            else
            {
                throw new FileTypeNotSupportedException($"File type {ext} is not supported.");
            }
        }

        public static string MapExtFromFileType(FileType fileType)
        {
            if (ExtFileTypeMap.TryGetValue(fileType, out string result))
            {
                return result;
            }
            else
            {
                throw new FileTypeNotSupportedException($"File type {fileType} is not supported.");
            }
        }

        public static bool ValidateExtIsImage(string ext)
        {
            if (FileTypeExtMap.TryGetValue(ext, out FileType result))
            {
                if (result == FileType.Jpeg || result == FileType.Jpg || result == FileType.Png)
                    return true;

                throw new FileTypeNotSupportedException($"File must be an image.");
            }
            else
            {
                throw new FileTypeNotSupportedException($"File type {ext} is not supported.");
            }
        }
    }
}
