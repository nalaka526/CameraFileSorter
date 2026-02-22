using ImageFileSorter.Infrastructure.FileTypeInfo;
using MetadataExtractor;
using MetadataExtractor.Formats.FileType;

namespace Image_File_Sorter
{
    internal static class FileReader
    {
        static readonly List<IFileTypeInfo> fileTypeInfoList = [];

        static FileReader()
        {
            fileTypeInfoList = new List<IFileTypeInfo> { new JpegInfo(), new Mp4Info() };
        }

        public static (bool canRead, DateTime createdDate) GetCreatedDateTime(string sourceFilePath)
        {
            string fileExt = Path.GetExtension(sourceFilePath);

            if (string.IsNullOrWhiteSpace(fileExt) || !IsSupportedFileType(fileExt))
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

        private static bool IsSupportedFileType(string fileExtention)
        {
            return fileTypeInfoList.Exists(e => e.FileExtensions.Contains(fileExtention));
        }

        private static IFileTypeInfo? GetFileTypeInfo(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            var fileType = directories.OfType<FileTypeDirectory>().FirstOrDefault()?.GetDescription(FileTypeDirectory.TagDetectedFileTypeName);
            return fileTypeInfoList.Where(e => e.FileTypeName == fileType).FirstOrDefault();
        }

        private static DateTime GetFileCreatedDateTime(string filePath, IFileTypeInfo fileTypeInfo)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            return fileTypeInfo.GetFileCreatedDateTime(directories);
        }
    }
}
