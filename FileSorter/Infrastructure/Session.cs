using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter.Infrastructure
{
    internal class Session
    {
        internal string SourcePath { get; set; }
        internal string TargetPath { get; set; }

        internal string DateSeparator { get; set; }
        internal bool CreateFolderForYear { get; set; }
        internal bool CreateFolderForMonth { get; set; }

        internal BackgroundWorker Worker;

        int currentFileIndex;
        public int SuccessFilesCount;
        public int FailedFilesCount;

        public Session(string sourcePath, string targetPath, string dateSeparator, bool createFolderForYear,  bool createFolderForMonth, BackgroundWorker worker)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            CreateFolderForYear = createFolderForYear;
            CreateFolderForMonth = createFolderForMonth;
            DateSeparator = dateSeparator;
            Worker = worker;
        }

        public void HandleFileProcessingStart(int fileIndex, string fileName)
        {
            currentFileIndex = fileIndex;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingStartMessage(fileIndex, fileName)));
        }

        public void HandleFileSkip()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileSkippedMessage(currentFileIndex), isSuccess: false, isWarning: true));
        }

        public void HandleFileProcessingFail()
        {
            FailedFilesCount++;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex), isSuccess: false));
        }

        public void HandleFileProcessingSuccess()
        {
            SuccessFilesCount++;
        }

        public void HandleFileCopyingSuccess(string destinationFolder)
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingSucessMessage(currentFileIndex, destinationFolder)));
        }

        public void HandleFileProcessingError(string? exceptionMessage = null)
        {
            FailedFilesCount++;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex, exceptionMessage), isSuccess: false));
        }

        public bool IsSucceeded()
        {
            return FailedFilesCount == 0;
        }
    }
}