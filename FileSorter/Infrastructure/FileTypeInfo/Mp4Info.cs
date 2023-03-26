using MetadataExtractor.Formats.QuickTime;
using System.Globalization;

namespace ImageFileSorter.Infrastructure.FileTypeInfo
{
    internal class Mp4Info : IFileTypeInfo
    {
        public string FileTypeName => "MP4";
        public string FileExtentions => ".mp4";
        private static string DateFormat => "ddd MMM dd HH:mm:ss yyyy";

        public DateTime GetFileCreatedDateTime(IEnumerable<MetadataExtractor.Directory> directories)
        {
            DateTime.TryParseExact(directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault()?.GetDescription(QuickTimeMovieHeaderDirectory.TagCreated),
                                    DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime createdDateTimeTest);
            return createdDateTimeTest;
        }
    }
}
