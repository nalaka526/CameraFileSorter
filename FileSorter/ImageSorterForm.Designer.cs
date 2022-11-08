namespace ImageFileSorter
{
    partial class ImageSorterForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnProcess = new System.Windows.Forms.Button();
            this.btnSourceFolder = new System.Windows.Forms.Button();
            this.btnTargetFolder = new System.Windows.Forms.Button();
            this.txtSourceFolder = new System.Windows.Forms.TextBox();
            this.txtTargetFolder = new System.Windows.Forms.TextBox();
            this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.lstLog = new System.Windows.Forms.ListView();
            this.Item = new System.Windows.Forms.ColumnHeader();
            this.chkYearFolder = new System.Windows.Forms.CheckBox();
            this.chkMonthFolder = new System.Windows.Forms.CheckBox();
            this.lblCreateFoldersFor = new System.Windows.Forms.Label();
            this.lblDateSeperaotor = new System.Windows.Forms.Label();
            this.cmbDateSeperator = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(361, 78);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(152, 46);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Sort";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.BtnProcess_Click);
            // 
            // btnSourceFolder
            // 
            this.btnSourceFolder.Location = new System.Drawing.Point(14, 13);
            this.btnSourceFolder.Name = "btnSourceFolder";
            this.btnSourceFolder.Size = new System.Drawing.Size(106, 23);
            this.btnSourceFolder.TabIndex = 3;
            this.btnSourceFolder.Text = "Source";
            this.btnSourceFolder.UseVisualStyleBackColor = true;
            this.btnSourceFolder.Click += new System.EventHandler(this.BtnSourceFolder_Click);
            // 
            // btnTargetFolder
            // 
            this.btnTargetFolder.Location = new System.Drawing.Point(14, 45);
            this.btnTargetFolder.Name = "btnTargetFolder";
            this.btnTargetFolder.Size = new System.Drawing.Size(106, 23);
            this.btnTargetFolder.TabIndex = 4;
            this.btnTargetFolder.Text = "Target";
            this.btnTargetFolder.UseVisualStyleBackColor = true;
            this.btnTargetFolder.Click += new System.EventHandler(this.BtnTargetFolder_Click);
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.BackColor = System.Drawing.SystemColors.Window;
            this.txtSourceFolder.Location = new System.Drawing.Point(126, 14);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.ReadOnly = true;
            this.txtSourceFolder.Size = new System.Drawing.Size(387, 23);
            this.txtSourceFolder.TabIndex = 5;
            // 
            // txtTargetFolder
            // 
            this.txtTargetFolder.BackColor = System.Drawing.SystemColors.Window;
            this.txtTargetFolder.Location = new System.Drawing.Point(126, 45);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.ReadOnly = true;
            this.txtTargetFolder.Size = new System.Drawing.Size(387, 23);
            this.txtTargetFolder.TabIndex = 6;
            // 
            // backgroundWorker
            // 
            this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker_DoWork);
            this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.BackgroundWorker_ProgressChanged);
            this.backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.BackgroundWorker_RunWorkerCompleted);
            // 
            // lstLog
            // 
            this.lstLog.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.lstLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Item});
            this.lstLog.GridLines = true;
            this.lstLog.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lstLog.Location = new System.Drawing.Point(9, 130);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(504, 299);
            this.lstLog.TabIndex = 7;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
            // 
            // Item
            // 
            this.Item.Text = "";
            this.Item.Width = 500;
            // 
            // chkYearFolder
            // 
            this.chkYearFolder.AutoSize = true;
            this.chkYearFolder.Checked = true;
            this.chkYearFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkYearFolder.Location = new System.Drawing.Point(126, 78);
            this.chkYearFolder.Name = "chkYearFolder";
            this.chkYearFolder.Size = new System.Drawing.Size(48, 19);
            this.chkYearFolder.TabIndex = 8;
            this.chkYearFolder.Text = "Year";
            this.chkYearFolder.UseVisualStyleBackColor = true;
            this.chkYearFolder.CheckedChanged += new System.EventHandler(this.ChkYearFolder_CheckedChanged);
            // 
            // chkMonthFolder
            // 
            this.chkMonthFolder.AutoSize = true;
            this.chkMonthFolder.Location = new System.Drawing.Point(177, 78);
            this.chkMonthFolder.Name = "chkMonthFolder";
            this.chkMonthFolder.Size = new System.Drawing.Size(62, 19);
            this.chkMonthFolder.TabIndex = 9;
            this.chkMonthFolder.Text = "Month";
            this.chkMonthFolder.UseVisualStyleBackColor = true;
            // 
            // lblCreateFoldersFor
            // 
            this.lblCreateFoldersFor.AutoSize = true;
            this.lblCreateFoldersFor.Location = new System.Drawing.Point(14, 79);
            this.lblCreateFoldersFor.Name = "lblCreateFoldersFor";
            this.lblCreateFoldersFor.Size = new System.Drawing.Size(82, 15);
            this.lblCreateFoldersFor.TabIndex = 10;
            this.lblCreateFoldersFor.Text = "Create Folders";
            // 
            // lblDateSeperaotor
            // 
            this.lblDateSeperaotor.AutoSize = true;
            this.lblDateSeperaotor.Location = new System.Drawing.Point(14, 104);
            this.lblDateSeperaotor.Name = "lblDateSeperaotor";
            this.lblDateSeperaotor.Size = new System.Drawing.Size(103, 15);
            this.lblDateSeperaotor.TabIndex = 11;
            this.lblDateSeperaotor.Text = "Folder Name Style";
            // 
            // cmbDateSeperator
            // 
            this.cmbDateSeperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateSeperator.FormattingEnabled = true;
            this.cmbDateSeperator.Location = new System.Drawing.Point(123, 101);
            this.cmbDateSeperator.MaxLength = 1;
            this.cmbDateSeperator.Name = "cmbDateSeperator";
            this.cmbDateSeperator.Size = new System.Drawing.Size(116, 23);
            this.cmbDateSeperator.TabIndex = 13;
            // 
            // ImageSorterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(519, 441);
            this.Controls.Add(this.cmbDateSeperator);
            this.Controls.Add(this.lblDateSeperaotor);
            this.Controls.Add(this.lblCreateFoldersFor);
            this.Controls.Add(this.chkMonthFolder);
            this.Controls.Add(this.chkYearFolder);
            this.Controls.Add(this.lstLog);
            this.Controls.Add(this.txtTargetFolder);
            this.Controls.Add(this.txtSourceFolder);
            this.Controls.Add(this.btnTargetFolder);
            this.Controls.Add(this.btnSourceFolder);
            this.Controls.Add(this.btnProcess);
            this.Name = "ImageSorterForm";
            this.Text = "Image File Sorter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button btnProcess;
        private Button btnSourceFolder;
        private Button btnTargetFolder;
        private TextBox txtSourceFolder;
        private TextBox txtTargetFolder;
        private System.ComponentModel.BackgroundWorker backgroundWorker;
        private ListView lstLog;
        private ColumnHeader Item;
        private CheckBox chkYearFolder;
        private CheckBox chkMonthFolder;
        private Label lblCreateFoldersFor;
        private Label lblDateSeperaotor;
        private ComboBox cmbDateSeperator;
    }
}