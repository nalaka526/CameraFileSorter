using ImageFileSorter.Infrastructure;
using ImageFileSorter.Infrastructure.Models;
using System.ComponentModel;

namespace ImageFileSorter
{
    public partial class ImageSorterForm : Form
    {
        string? sourcePath;
        string? targetPath;
        string? seperator;

        bool isProcessing;
        bool isCancelled;
        bool isError;

        internal BackgroundWorker? worker;
        internal Session currentSession;

        public ImageSorterForm()
        {
            InitializeComponent();

            PopulateFolderNameFormats();

            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;

            txtSourceFolder.Text = sourcePath;
            txtTargetFolder.Text = targetPath;
            txtTargetFolder.Text = targetPath;
        }

        #region Background Worker

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            worker = sender as BackgroundWorker;

            if (sourcePath == null || targetPath == null || seperator == null || worker == null)
                return;

            currentSession = new Session(sourcePath,
                                                    targetPath,
                                                    seperator,
                                                    chkYearFolder.Checked,
                                                    chkMonthFolder.Checked,
                                                    worker);

            var sorter = new FileSorter(currentSession);

            sorter.Sort();
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            if (e.UserState is not UserState state)
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

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
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
                if (currentSession.IsSucceeded())
                    Log(LogHelper.GetSessionSucessMessage(currentSession.SuccessFilesCount));
                else
                    Log(LogHelper.GetSessionFailedMessage(currentSession.SuccessFilesCount, currentSession.FailedFilesCount));

                Log(LogHelper.GetSessionSucessMessage());
            }
        }

        #endregion

        #region User Events

        private void BtnSourceFolder_Click(object sender, EventArgs e)
        {
            using var fbd = new FolderBrowserDialog();
            fbd.InitialDirectory = sourcePath;
            DialogResult result = fbd.ShowDialog();

            if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                sourcePath = fbd.SelectedPath;
                txtSourceFolder.Text = sourcePath;
                txtTargetFolder.Text = sourcePath + "_sorted";
            }
        }

        private void BtnTargetFolder_Click(object sender, EventArgs e)
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

        private void BtnProcess_Click(object sender, EventArgs e)
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
                        seperator = cmbDateSeperator.SelectedValue.ToString();
                        btnProcess.Text = "Stop Sorting";

                        this.backgroundWorker.RunWorkerAsync();
                    }
                }
            }
        }

        private void ChkYearFolder_CheckedChanged(object sender, EventArgs e)
        {
            chkMonthFolder.Enabled = chkYearFolder.Checked;
            chkMonthFolder.Checked = false;
        }

        #endregion

        #region Other

        private void PopulateFolderNameFormats()
        {
            cmbDateSeperator.DisplayMember = "Text";
            cmbDateSeperator.ValueMember = "Value";

            var items = new[] {
                new { Text = "ddMMyyyy", Value = string.Empty },
                new { Text = "dd.MM.yyyy", Value = "." },
                new { Text = "dd-MM-yyyy", Value = "-" }
            };

            cmbDateSeperator.DataSource = items;

            cmbDateSeperator.SelectedIndex = 1;
        }

        private bool ValidateProcessStart()
        {

            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath))
            {
                Log(LogHelper.GetSeperaotor());
                LogWarning(LogHelper.GetValidationEmptyFolderPathsMessage());
                return false;
            }

            if (targetPath == sourcePath)
            {
                Log(LogHelper.GetSeperaotor());
                LogWarning(LogHelper.GetValidationInvalidFolderPathsMessage());
                return false;
            }

            if (!Directory.Exists(sourcePath))
            {
                Log(LogHelper.GetSeperaotor());
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