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

                string? destFolder = null;
                string fileExt = Path.GetExtension(sourceFilePath);

                if (string.IsNullOrWhiteSpace(fileExt) || !fileTypeInfoList.Exists(e => e.FileExtentions.Contains(fileExt)))
                {
                    destFolder = Path.Combine(CurrentSession.TargetPath, "NotHandled");
                    CurrentSession.HandleFileSkip();
                }
                else
                {
                    IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(sourceFilePath);

                    var fileType = directories.OfType<FileTypeDirectory>().FirstOrDefault()?.GetDescription(FileTypeDirectory.TagDetectedFileTypeName);

                    var fileTypeInfo = fileTypeInfoList.Where(e => e.FileTypeName == fileType).FirstOrDefault();

                    if (fileTypeInfo == null)
                    {
                        destFolder = Path.Combine(CurrentSession.TargetPath, "NotHandled");
                        CurrentSession.HandleFileSkip();
                    }
                    else
                    {
                        var createdDateTime = fileTypeInfo.GetFileCreatedDateTime(directories);

                        if (createdDateTime == default || createdDateTime < new DateTime(1900, 1, 1))
                        {
                            destFolder = Path.Combine(CurrentSession.TargetPath, "Failed");
                            CurrentSession.HandleFileProcessingFail();
                        }
                        else if (lastDate.Date != createdDateTime.Date || destFolder == null)
                        {
                            destFolder = destFolder = Path.Combine(CurrentSession.TargetPath,
                                            CurrentSession.CreateFolderForYear ? createdDateTime.Year.ToString() : string.Empty,
                                            CurrentSession.CreateFolderForMonth ? createdDateTime.Month.ToString().PadLeft(2, '0') : string.Empty,
                                            createdDateTime.Year.ToString() + CurrentSession.DateSeperator +
                                            createdDateTime.Month.ToString().PadLeft(2, '0') + CurrentSession.DateSeperator +
                                            createdDateTime.Day.ToString().PadLeft(2, '0'));
                            CurrentSession.HandleFileProcessingSucess();
                            lastDate = createdDateTime;
                        }
                    }
                }

                System.IO.Directory.CreateDirectory(destFolder);
                File.Copy(sourceFilePath, Path.Combine(destFolder, fileName), true);

                CurrentSession.HandleFileMovingSuccess(destFolder);

            }
            catch (Exception)
            {
                CurrentSession.HandleFileProcessingError();
                return;
            }
        }
    }
}
