using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter.Infrastructure
{
    internal class Session
    {
        internal string SourcePath { get; set; }
        internal string TargetPath { get; set; }

        internal string DateSeperator { get; set; }
        internal bool CreateFolderForYear { get; set; }
        internal bool CreateFolderForMonth { get; set; }

        internal BackgroundWorker Worker;

        int currentFileIndex;
        public int SuccessFilesCount;
        public int FailedFilesCount;

        public Session(string sourcePath, string targetPath, string dateSeperator, bool createFolderForYear,  bool createFolderForMonth, BackgroundWorker worker)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            CreateFolderForYear = createFolderForYear;
            CreateFolderForMonth = createFolderForMonth;
            DateSeperator = dateSeperator;
            Worker = worker;
        }

        public void HandleFileProcessingStart(int fileIndex, string fileName)
        {
            currentFileIndex = fileIndex;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingStartMessage(fileIndex, fileName)));
        }

        public void HandleFileSkip()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileSkippedMessage(currentFileIndex)));
        }

        public void HandleFileProcessingFail()
        {
            FailedFilesCount++;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex), false));
        }

        public void HandleFileProcessingSucess()
        {
            SuccessFilesCount++;
        }

        public void HandleFileMovingSuccess(string destinationFolder)
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingSucessMessage(currentFileIndex, destinationFolder)));
        }

        public void HandleFileProcessingError()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex), false));
        }
    }
}