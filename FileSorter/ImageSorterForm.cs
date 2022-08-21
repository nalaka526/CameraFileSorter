using MetadataExtractor;
using MetadataExtractor.Formats.Exif;
using MetadataExtractor.Formats.FileType;
using MetadataExtractor.Formats.QuickTime;
using System.ComponentModel;
using System.Globalization;

namespace FileSorter
{
    public partial class ImageSorterForm : Form
    {
        string sourcePath;
        string targetPath;

        static readonly string jpegFileTypeName = "JPEG";
        static readonly string jpegDateFormat = "yyyy:MM:dd HH:mm:ss";

        static readonly string mp4FileTypeName = "MP4";
        static readonly string mp4DateFormat = "ddd MMM dd HH:mm:ss yyyy";

        int fileCount;
        bool isProcessing;
        bool isCancelled;
        bool isError;

        BackgroundWorker worker;

        public ImageSorterForm()
        {
            InitializeComponent();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            txtSourceFolder.Text = sourcePath;
            txtTargetFolder.Text = targetPath;
        }

        #region Background Worker

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            if (worker == null)
                return;

            worker.ReportProgress(0, new UserState($"Source folder {sourcePath}"));
            worker.ReportProgress(0, new UserState($"Target folder {targetPath}"));

            Sort();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is UserState)
            {
                var state = e.UserState as UserState;

                if (state == null)
                    return;

                if (state.Sucess)
                {
                    Log($"{state.Message}");
                }
                else
                {
                    LogError($"{state.Message}");
                }

            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (isError)
            {
                HandleProcessEnd("Error occured");
            }
            else if (isCancelled)
            {
                HandleProcessEnd("Sorting canceled");
            }
            else
            {
                HandleProcessEnd("Sorting completed");
            }
        }

        #endregion

        #region User Events

        private void btnSourceFolder_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.InitialDirectory = sourcePath;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                sourcePath = fbd.SelectedPath;
                txtSourceFolder.Text = sourcePath;

                if (string.IsNullOrEmpty(targetPath))
                {
                    targetPath = fbd.SelectedPath;
                    txtTargetFolder.Text = sourcePath;
                }
            }
        }

        private void btnTargetFolder_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.InitialDirectory = targetPath;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                targetPath = fbd.SelectedPath;
                txtTargetFolder.Text = targetPath;
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath))
            {
                HandleValidationError("Invalid folder locations");
                return;
            }

            if (sourcePath == targetPath)
            {
                HandleValidationError("Source and destination folders should be different");
                return;
            }

            if (isProcessing)
            {
                if (backgroundWorker.WorkerSupportsCancellation == true)
                {
                    isError = false;
                    isCancelled = true;
                    backgroundWorker.CancelAsync();
                }
            }
            else
            {
                if (backgroundWorker.IsBusy != true)
                {
                    HandleProcessStart();
                    this.backgroundWorker.RunWorkerAsync();
                }
            }
        }

        #endregion

        #region Support

        private void HandleValidationError(string message)
        {
            Log("-----------------------------------------------------------------------------------------------", false);
            LogWarning(message);
        }

        private void HandleProcessStart()
        {
            isCancelled = false;
            isProcessing = true;
            fileCount = 0;
            btnProcess.Text = "Stop Sorting";

            Log("-----------------------------------------------------------------------------------------------", false);
            Log("Sorting started");
        }

        private void HandleProcessEnd(string message)
        {
            btnProcess.Text = "Sort";
            isProcessing = false;

            Log(message);
        }

        private void Log(string message, bool logTime = true)
        {
            if (logTime)
            {
                lstLog.Items.Insert(0, message);
                return;
            }

            lstLog.Items.Insert(0, message);
        }

        private void LogWarning(string message)
        {
            ListViewItem li = new ListViewItem();
            li.ForeColor = Color.OrangeRed;
            li.Text = message;
            lstLog.Items.Insert(0, li);
        }


        private void LogError(string message)
        {
            ListViewItem li = new ListViewItem();
            li.ForeColor = Color.Red;
            li.Text = message;
            lstLog.Items.Insert(0, li);
        }

        #endregion

        #region Process

        public void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = System.IO.Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                if (worker.CancellationPending == true)
                {
                    return;
                }

                ProcessFile(fileName);
            }

            string[] subdirectoryEntries = System.IO.Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
            {
                if (worker.CancellationPending == true)
                {
                    return;
                }

                ProcessDirectory(subdirectory);
            }

        }

        public void ProcessFile(string sourceFilePath)
        {
            fileCount++;

            var fileName = Path.GetFileName(sourceFilePath);

            try
            {
                worker.ReportProgress(0, new UserState($"File {fileCount} : {fileName}"));

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
                    worker.ReportProgress(0, new UserState($"File '{fileName}' sorting failed", false));
                    return;
                }

                var destFolder = Path.Combine(targetPath, createdDateTime.Year.ToString(),
                                                            createdDateTime.Year.ToString() + "." +
                                                            createdDateTime.Month.ToString().PadLeft(2, '0') + "." +
                                                            createdDateTime.Day.ToString().PadLeft(2, '0'));

                System.IO.Directory.CreateDirectory(destFolder);

                string destFilePath = Path.Combine(destFolder, fileName);
                File.Copy(sourceFilePath, destFilePath, true);

                worker.ReportProgress(0, new UserState($"File copied to '{destFolder}'"));

            }
            catch (Exception)
            {
                worker.ReportProgress(0, new UserState($"Error occured while sorting file '{fileName}'", false));
                return;
            }
        }

        private void Sort()
        {
            if (System.IO.Directory.Exists(sourcePath))
            {
                ProcessDirectory(sourcePath);
            }
            else
            {
                isError = true;
                worker.ReportProgress(0, new UserState($"'{sourcePath}' is not a valid directory."));
            }
        }

        #endregion

    }

    public class ListBoxItem
    {
        public ListBoxItem(Color c, string m)
        {
            ItemColor = c;
            Message = m;
        }
        public Color ItemColor { get; set; }
        public string Message { get; set; }
    }

    public class UserState
    {
        public UserState(string message, bool sucess = true)
        {
            Message = message;
            Sucess = sucess;
        }

        public string Message { get; set; }
        public bool Sucess { get; set; }
    }
}