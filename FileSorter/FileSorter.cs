using ImageFileSorter.Infrastructure;
using ImageFileSorter.Infrastructure.FileTypeInfo;
using MetadataExtractor;
using MetadataExtractor.Formats.FileType;

namespace ImageFileSorter
{
    internal class FileSorter
    {
        readonly Session currentSession;
        int fileCount;

        List<IFileTypeInfo> fileTypeInfoList = [];
        DateTime lastDate = default;

        private readonly string skipDestinationPath;
        private readonly string failedDestinationPath;

        public FileSorter(Session currentSession)
        {
            this.currentSession = currentSession;
            skipDestinationPath = GetSkipDestinationPath();
            failedDestinationPath = GetFailedDestinationPath();
        }

        public void Sort()
        {
            fileCount = 0;
            fileTypeInfoList = new List<IFileTypeInfo> { new JpegInfo(), new Mp4Info() };

            ProcessDirectory(currentSession.SourcePath);
        }

        private void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = System.IO.Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (currentSession.Worker.CancellationPending == true)
                {
                    return;
                }

                ProcessFile(fileName);
            }
        }

        private void ProcessFile(string sourceFilePath)
        {
            fileCount++;

            var fileName = Path.GetFileName(sourceFilePath);

            try
            {
                this.currentSession.HandleFileProcessingStart(fileCount, fileName);

                string? destFolder = null;
                string fileExt = Path.GetExtension(sourceFilePath);

                if (string.IsNullOrWhiteSpace(fileExt) || !fileTypeInfoList.Exists(e => e.FileExtentions.Contains(fileExt)))
                {
                    this.currentSession.HandleFileSkip();
                    MoveFile(sourceFilePath, fileName, skipDestinationPath);
                    return;
                }

                var fileTypeInfo = GetFileTypeInfo(sourceFilePath);

                if (fileTypeInfo == null)
                {
                    currentSession.HandleFileSkip();
                    MoveFile(sourceFilePath, fileName, skipDestinationPath);
                    return;
                }

                DateTime createdDateTime = GetFileCreatedDateTime(sourceFilePath, fileTypeInfo);

                if (createdDateTime == default || createdDateTime < new DateTime(1900, 1, 1))
                {
                    currentSession.HandleFileProcessingFail();
                    MoveFile(sourceFilePath, fileName, failedDestinationPath);
                }
                else if (lastDate.Date != createdDateTime.Date || destFolder == null)
                {
                    destFolder = GetSuccessDestinationPath(createdDateTime);
                    currentSession.HandleFileProcessingSuccess();
                    MoveFile(sourceFilePath, fileName, destFolder);
                    lastDate = createdDateTime;
                }
            }
            catch (Exception)
            {
                currentSession.HandleFileProcessingError();
                return;
            }
        }

        private string GetSuccessDestinationPath(DateTime createdDateTime)
        {
            return Path.Combine(currentSession.TargetPath,
                            currentSession.CreateFolderForYear ? createdDateTime.Year.ToString() : string.Empty,
                            currentSession.CreateFolderForMonth ? createdDateTime.Month.ToString().PadLeft(2, '0') : string.Empty,
                            createdDateTime.Year.ToString() + currentSession.DateSeperator +
                            createdDateTime.Month.ToString().PadLeft(2, '0') + currentSession.DateSeperator +
                            createdDateTime.Day.ToString().PadLeft(2, '0'));
        }

        private string GetSkipDestinationPath()
        {
            return Path.Combine(currentSession.TargetPath, "NotHandled");
        }

        private string GetFailedDestinationPath()
        {
            return Path.Combine(currentSession.TargetPath, "Failed");
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

            currentSession.HandleFileMovingSuccess(destFolder);
        }
    }
}
