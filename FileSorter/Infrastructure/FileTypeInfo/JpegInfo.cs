using MetadataExtractor.Formats.Exif;
using System.Globalization;

namespace ImageFileSorter.Infrastructure.FileTypeInfo
{
    internal class JpegInfo : IFileTypeInfo
    {
        public string FileTypeName => "JPEG";
        public string FileExtentions => ".jpg.jpeg";
        private static string DateFormat => "yyyy:MM:dd HH:mm:ss";

        public DateTime GetFileCreatedDateTime(IEnumerable<MetadataExtractor.Directory> directories)
        {
            DateTime.TryParseExact(directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal),
                                    DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdDateTimeTest);
            return createdDateTimeTest;
        }
    }
}
