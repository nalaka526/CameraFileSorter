using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter
{
    public partial class ImageSorterForm : Form
    {
        string? sourcePath;
        string? targetPath;

        bool isProcessing;
        bool isCancelled;
        bool isError;

        BackgroundWorker? worker;

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

            if (sourcePath == null || targetPath == null || worker == null)
                return;

            worker.ReportProgress(0, new UserState($"Source folder {sourcePath}"));
            worker.ReportProgress(0, new UserState($"Target folder {targetPath}"));

            var sorter = new FileSorter(sourcePath, targetPath, worker);

            sorter.Sort();
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState is UserState)
            {
                var state = e.UserState as UserState;

                if (state == null)
                    return;

                if (state.IsSucess)
                {
                    Log($"{state.CreatedOn.ToString("HH:mm:ss:fff") + " : " + state.Message}");
                }
                else
                {
                    LogError($"{state.CreatedOn.ToString("HH:mm:ss:fff") + " : " + state.Message}");
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
            var result = fbd.ShowDialog();

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
                HandleValidationError("Empty folder locations");
                return;
            }

            //if (sourcePath.StartsWith(targetPath) || targetPath.StartsWith(sourcePath))
            //{
            //    HandleValidationError("Invalid Source & Target folder");
            //    return;
            //}

            if (!Directory.Exists(sourcePath))
            {
                HandleValidationError($"'{sourcePath}' is not a valid directory.");
                return;
            }

            if (isProcessing)
            {
                if (backgroundWorker.WorkerSupportsCancellation == true)
                {
                    isError = false;
                    isCancelled = true;
                    backgroundWorker.CancelAsync();

                    btnProcess.Text = "Cancelling...";
                    btnProcess.Enabled = false;
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
            Log("-----------------------------------------------------------------------------------------------");
            LogWarning(message);
        }

        private void HandleProcessStart()
        {
            isCancelled = false;
            isProcessing = true;
            btnProcess.Text = "Stop Sorting";

            Log("-----------------------------------------------------------------------------------------------");
            Log("Sorting started");
        }

        private void HandleProcessEnd(string message)
        {
            btnProcess.Text = "Sort";
            isProcessing = false;

            btnProcess.Enabled = true;

            Log(message);
        }

        private void Log(string message)
        {
            lstLog.Items.Insert(0, message);
        }

        private void LogWarning(string message)
        {
            lstLog.Items.Insert(0, new ListViewItem
            {
                ForeColor = Color.OrangeRed,
                Text = message
            });
        }


        private void LogError(string message)
        {
            lstLog.Items.Insert(0, new ListViewItem
            {
                ForeColor = Color.Red,
                Text = message
            });
        }

        #endregion

        #region Process



        #endregion

    }
}