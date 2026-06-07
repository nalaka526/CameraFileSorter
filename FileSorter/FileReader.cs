using ImageFileSorter.Infrastructure.FileTypeInfo;
using MetadataExtractor;
using MetadataExtractor.Formats.FileType;

namespace Image_File_Sorter
{
    internal static class FileReader
    {
        static readonly List<IFileTypeInfo> fileTypeInfos = new List<IFileTypeInfo> { new JpegInfo(), new Mp4Info() };

        static FileReader()
        {
        }

        public static (bool canRead, DateTime createdDate) GetCreatedDateTime(string sourceFilePath)
        {
            string fileExtension = Path.GetExtension(sourceFilePath);

            if (string.IsNullOrWhiteSpace(fileExtension) || !IsSupportedFileType(fileExtension))
            {
                return (false, default(DateTime));
            }

            var fileTypeInfo = GetFileTypeInfo(sourceFilePath);

            if (fileTypeInfo == null)
            {
                return (false, default(DateTime));
            }

            return (true, GetFileCreatedDateTime(sourceFilePath, fileTypeInfo));
        }

        private static bool IsSupportedFileType(string fileExtension)
        {
            return fileTypeInfos.Exists(e => e.FileExtensions.Contains(fileExtension, StringComparer.OrdinalIgnoreCase));
        }

        private static IFileTypeInfo? GetFileTypeInfo(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            var fileType = directories.OfType<FileTypeDirectory>().FirstOrDefault()?.GetDescription(FileTypeDirectory.TagDetectedFileTypeName);
            return fileTypeInfos.FirstOrDefault(e => e.FileTypeName == fileType);
        }

        private static DateTime GetFileCreatedDateTime(string filePath, IFileTypeInfo fileTypeInfo)
        {
            return fileTypeInfo.GetFileCreatedDateTime(ImageMetadataReader.ReadMetadata(filePath));
        }
    }
}
