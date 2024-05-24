using ImageFileSorter.Infrastructure;
using ImageFileSorter.Infrastructure.FileTypeInfo;
using MetadataExtractor;
using MetadataExtractor.Formats.FileType;

namespace ImageFileSorter
{
    internal class FileSorter
    {
        readonly Session CurrentSession;
        int fileCount;

        List<IFileTypeInfo> fileTypeInfoList = new();
        DateTime lastDate = default;


        public FileSorter(Session currentSession)
        {
            CurrentSession = currentSession;
        }

        public void Sort()
        {
            fileCount = 0;
            fileTypeInfoList = new List<IFileTypeInfo> { new JpegInfo(), new Mp4Info() };

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
        }

        public void ProcessFile(string sourceFilePath)
        {
            fileCount++;

            var fileName = Path.GetFileName(sourceFilePath);

            try
            {
                CurrentSession.HandleFileProcessingStart(fileCount, fileName);

                string? destFolder = null;
                string fileExt = Path.GetExtension(sourceFilePath);

                if (string.IsNullOrWhiteSpace(fileExt) || !fileTypeInfoList.Exists(e => e.FileExtentions.Contains(fileExt)))
                {
                    destFolder = GetSkipDestinationPath();
                    CurrentSession.HandleFileSkip();
                    MoveFile(sourceFilePath, fileName, destFolder);
                    return;
                }

                var fileTypeInfo = GetFileTypeInfo(sourceFilePath);

                if (fileTypeInfo == null)
                {
                    destFolder = destFolder = GetSkipDestinationPath();
                    CurrentSession.HandleFileSkip();
                    MoveFile(sourceFilePath, fileName, destFolder);
                    return;
                }

                DateTime createdDateTime = GetFileCreatedDateTime(sourceFilePath, fileTypeInfo);

                if (createdDateTime == default || createdDateTime < new DateTime(1900, 1, 1))
                {
                    destFolder = GetFailedDestinationPath();
                    CurrentSession.HandleFileProcessingFail();
                    MoveFile(sourceFilePath, fileName, destFolder);
                }
                else if (lastDate.Date != createdDateTime.Date || destFolder == null)
                {
                    destFolder = GetSuccessDestinationPath(createdDateTime);
                    CurrentSession.HandleFileProcessingSuccess();
                    MoveFile(sourceFilePath, fileName, destFolder);
                    lastDate = createdDateTime;
                }
            }
            catch (Exception)
            {
                CurrentSession.HandleFileProcessingError();
                return;
            }
        }
        
        private string GetSuccessDestinationPath(DateTime createdDateTime)
        {
            return Path.Combine(CurrentSession.TargetPath,
                            CurrentSession.CreateFolderForYear ? createdDateTime.Year.ToString() : string.Empty,
                            CurrentSession.CreateFolderForMonth ? createdDateTime.Month.ToString().PadLeft(2, '0') : string.Empty,
                            createdDateTime.Year.ToString() + CurrentSession.DateSeperator +
                            createdDateTime.Month.ToString().PadLeft(2, '0') + CurrentSession.DateSeperator +
                            createdDateTime.Day.ToString().PadLeft(2, '0'));
        }

        private string GetSkipDestinationPath()
        {
            return Path.Combine(CurrentSession.TargetPath, "NotHandled");
        }

        private string GetFailedDestinationPath()
        {
            return Path.Combine(CurrentSession.TargetPath, "Failed");
        }

        private static DateTime GetFileCreatedDateTime(string filePath, IFileTypeInfo fileTypeInfo)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            return fileTypeInfo.GetFileCreatedDateTime(directories);
        }

        private IFileTypeInfo? GetFileTypeInfo(string filePath)
        {
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            var fileType = directories.OfType<FileTypeDirectory>().FirstOrDefault()?.GetDescription(FileTypeDirectory.TagDetectedFileTypeName);
            return fileTypeInfoList.Where(e => e.FileTypeName == fileType).FirstOrDefault();
        }

        private void MoveFile(string sourceFilePath, string fileName, string destFolder)
        {
            System.IO.Directory.CreateDirectory(destFolder);
            File.Copy(sourceFilePath, Path.Combine(destFolder, fileName), true);

            CurrentSession.HandleFileMovingSuccess(destFolder);
        }
    }
}
