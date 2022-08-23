using ImageFileSorter.Infrastructure;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.QuickTime;
using System.Globalization;

namespace ImageFileSorter
{
    public class FileSorter
    {
        Session CurrentSession;

        static readonly string jpegFileTypeName = "JPEG";
        static readonly string jpegDateFormat = "yyyy:MM:dd HH:mm:ss";

        static readonly string mp4FileTypeName = "MP4";
        static readonly string mp4DateFormat = "ddd MMM dd HH:mm:ss yyyy";

        int fileCount;

        DateTime lastDate = default;
        string? destFolder;

        public FileSorter(Session currentSession)
        {
            CurrentSession = currentSession;
        }

        public void Sort()
        {
            fileCount = 0;
            ProcessDirectory(CurrentSession.SourcePath);
        }

        public void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = System.IO.Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (CurrentSession.Worker.CancellationPending == true)
                {
                    return;
                }

                ProcessFile(fileName);
            }

            //string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
            //foreach (string subdirectory in subdirectoryEntries)
            //{
            //    if (Worker.CancellationPending == true)
            //    {
            //        return;
            //    }

            //    ProcessDirectory(subdirectory);
            //}

        }

        public void ProcessFile(string sourceFilePath)
        {
            fileCount++;

            var fileName = Path.GetFileName(sourceFilePath);

            try
            {
                CurrentSession.HandleFileProcessingStart(fileCount, fileName);

                IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(sourceFilePath);

                var fileType = directories.OfType<FileTypeDirectory>().FirstOrDefault()?.GetDescription(FileTypeDirectory.TagDetectedFileTypeName);

                var isSuccess = false;
                DateTime createdDateTime = default;
                string? dateTime;

                if (fileType == jpegFileTypeName)
                {
                    dateTime = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault()?.GetDescription(ExifDirectoryBase.TagDateTimeOriginal);
                    isSuccess = DateTime.TryParseExact(dateTime, jpegDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDateTime);
                }
                else if (fileType == mp4FileTypeName)
                {
                    dateTime = directories.OfType<QuickTimeMovieHeaderDirectory>().FirstOrDefault()?.GetDescription(QuickTimeMovieHeaderDirectory.TagCreated);
                    isSuccess = DateTime.TryParseExact(dateTime, mp4DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out createdDateTime);
                }

                if (!isSuccess || createdDateTime == default)
                {
                    CurrentSession.HandleFileProcessingFail();
                    return;
                }

                if (lastDate.Date != createdDateTime.Date || destFolder == null)
                {
                    destFolder = Path.Combine(CurrentSession.TargetPath, createdDateTime.Year.ToString(),
                                            createdDateTime.Year.ToString() + "." +
                                            createdDateTime.Month.ToString().PadLeft(2, '0') + "." +
                                            createdDateTime.Day.ToString().PadLeft(2, '0'));

                    System.IO.Directory.CreateDirectory(destFolder);

                    lastDate = createdDateTime;
                }

                string destFilePath = Path.Combine(destFolder, fileName);
                File.Copy(sourceFilePath, destFilePath, true);

                CurrentSession.HandleFileProcessingSucess();

            }
            catch (Exception)
            {
                CurrentSession.HandleFileProcessingError();
                return;
            }
        }
    }
}
