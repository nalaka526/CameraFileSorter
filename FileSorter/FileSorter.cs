using Image_File_Sorter;
using ImageFileSorter.Infrastructure;

namespace ImageFileSorter
{
    internal class FileSorter
    {
        readonly Session currentSession;
        int fileCount;

        DateTime lastDate = default;

        private readonly string skipDestinationPath;
        private readonly string failedDestinationPath;

        string? destFolder = null;

        public FileSorter(Session currentSession)
        {
            this.currentSession = currentSession;
            skipDestinationPath = GetSkipFolderPath();
            failedDestinationPath = GetFailedFolderPath();
        }

        public void Sort()
        {
            ProcessDirectory(currentSession.SourcePath);
        }

        private void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);

            fileCount = 0;

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
                currentSession.HandleFileProcessingStart(fileCount, fileName);

                var (canRead, createdDateTime) = FileReader.GetCreatedDateTime(sourceFilePath);

                if (!canRead)
                {
                    currentSession.HandleFileSkip();
                    MoveFile(sourceFilePath, fileName, skipDestinationPath);
                    return;
                }

                if (createdDateTime == default || createdDateTime < new DateTime(1900, 1, 1))
                {
                    currentSession.HandleFileProcessingFail();
                    MoveFile(sourceFilePath, fileName, failedDestinationPath);
                    return;
                }

                if (lastDate.Date != createdDateTime.Date || destFolder == null)
                {
                    destFolder = GetTargetFolderPath(createdDateTime);
                }

                currentSession.HandleFileProcessingSuccess();
                MoveFile(sourceFilePath, fileName, destFolder);
                lastDate = createdDateTime;
            }
            catch (Exception)
            {
                currentSession.HandleFileProcessingError();
                return;
            }
        }

        private string GetTargetFolderPath(DateTime createdDateTime)
        {
            return Path.Combine(currentSession.TargetPath,
                            currentSession.CreateFolderForYear ? createdDateTime.Year.ToString() : string.Empty,
                            currentSession.CreateFolderForMonth ? createdDateTime.Month.ToString().PadLeft(2, '0') : string.Empty,
                            createdDateTime.Year.ToString() + currentSession.DateSeperator +
                            createdDateTime.Month.ToString().PadLeft(2, '0') + currentSession.DateSeperator +
                            createdDateTime.Day.ToString().PadLeft(2, '0'));
        }

        private string GetSkipFolderPath()
        {
            return Path.Combine(currentSession.TargetPath, "NotHandled");
        }

        private string GetFailedFolderPath()
        {
            return Path.Combine(currentSession.TargetPath, "Failed");
        }

        private void MoveFile(string sourceFilePath, string fileName, string destFolder)
        {
            Directory.CreateDirectory(destFolder);
            File.Copy(sourceFilePath, Path.Combine(destFolder, fileName), true);

            currentSession.HandleFileMovingSuccess(destFolder);
        }
    }
}
