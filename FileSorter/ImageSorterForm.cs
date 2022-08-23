using ImageFileSorter.Infrastructure;
using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter
{
    public partial class ImageSorterForm : Form
    {
        string sourcePath;
        string targetPath;

        bool isProcessing;
        bool isCancelled;
        bool isError;

        internal BackgroundWorker? worker;

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

            var sorter = new FileSorter(new Session(sourcePath, targetPath, worker));

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
                    Log($"{state.Message}");
                }
                else if (state.IsWarning)
                {
                    LogWarning($"{state.Message}");
                }
                else
                {
                    LogError($"{state.Message}");
                }
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnProcess.Text = "Sort";
            isProcessing = false;

            btnProcess.Enabled = true;

            if (isError)
            {
                LogError(LogHelper.GetSessionErrorMessage());
            }
            else if (isCancelled)
            {
                LogWarning(LogHelper.GetSessionCancelMessage());
            }
            else
            {
                Log(LogHelper.GetSessionSucessMessage());
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
                    if (ValidateProcessStart())
                    {
                        Log(LogHelper.GetSeperaotor());
                        Log(LogHelper.GetSessionStartMessage());
                        Log(LogHelper.GetSourceFolderPathMessage(sourcePath));
                        Log(LogHelper.GetSTargetFolderPathMessage(targetPath));

                        isCancelled = false;
                        isProcessing = true;
                        btnProcess.Text = "Stop Sorting";

                        this.backgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        #endregion

        #region Other

        private bool ValidateProcessStart()
        {

            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath))
            {
                LogWarning(LogHelper.GetSeperaotor());
                LogWarning(LogHelper.GetValidationEmptyFolderPathsMessage());
                return false;
            }

            if (sourcePath.StartsWith(targetPath) || targetPath.StartsWith(sourcePath))
            {
                LogWarning(LogHelper.GetSeperaotor());
                LogWarning(LogHelper.GetValidationInvalidFolderPathsMessage());
                return false;
            }

            if (!Directory.Exists(sourcePath))
            {
                LogWarning(LogHelper.GetSeperaotor());
                LogWarning(LogHelper.GetValidationInvalidSourceFolderPathMessage(sourcePath));
                return false;
            }

            return true;
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

    }
}