using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter.Infrastructure
{
    public class Session
    {
        internal string SourcePath { get; set; }
        internal string TargetPath { get; set; }
        internal BackgroundWorker Worker;

        int currentFileIndex;
        string currentFileName;

        public Session(string sourcePath, string targetPath, BackgroundWorker worker)
        {
            SourcePath = sourcePath;
            TargetPath = targetPath;
            Worker = worker;
        }

        public void HandleFileProcessingStart(int fileIndex, string fileName)
        {
            currentFileIndex = fileIndex;
            currentFileName = fileName;
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingStartMessage(fileIndex, fileName)));
        }

        public void HandleFileProcessingFail()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex), false));
        }

        public void HandleFileProcessingSucess()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingSucessMessage(currentFileIndex, currentFileName)));
        }

        public void HandleFileProcessingError()
        {
            Worker.ReportProgress(0, new UserState(LogHelper.GetFileProcessingErrorMessage(currentFileIndex), false));
        }


    }
}