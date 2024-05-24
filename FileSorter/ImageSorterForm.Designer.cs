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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImageSorterForm));
            btnProcess = new Button();
            btnSourceFolder = new Button();
            btnTargetFolder = new Button();
            txtSourceFolder = new TextBox();
            txtTargetFolder = new TextBox();
            backgroundWorker = new System.ComponentModel.BackgroundWorker();
            lstLog = new ListView();
            Item = new ColumnHeader();
            chkYearFolder = new CheckBox();
            chkMonthFolder = new CheckBox();
            lblCreateFoldersFor = new Label();
            lblDateSeperaotor = new Label();
            cmbDateSeperator = new ComboBox();
            SuspendLayout();
            // 
            // btnProcess
            // 
            btnProcess.Location = new Point(361, 78);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(152, 46);
            btnProcess.TabIndex = 0;
            btnProcess.Text = "Sort";
            btnProcess.UseVisualStyleBackColor = true;
            btnProcess.Click += BtnProcess_Click;
            // 
            // btnSourceFolder
            // 
            btnSourceFolder.Location = new Point(14, 13);
            btnSourceFolder.Name = "btnSourceFolder";
            btnSourceFolder.Size = new Size(106, 23);
            btnSourceFolder.TabIndex = 3;
            btnSourceFolder.Text = "Source";
            btnSourceFolder.UseVisualStyleBackColor = true;
            btnSourceFolder.Click += BtnSourceFolder_Click;
            // 
            // btnTargetFolder
            // 
            btnTargetFolder.Location = new Point(14, 45);
            btnTargetFolder.Name = "btnTargetFolder";
            btnTargetFolder.Size = new Size(106, 23);
            btnTargetFolder.TabIndex = 4;
            btnTargetFolder.Text = "Target";
            btnTargetFolder.UseVisualStyleBackColor = true;
            btnTargetFolder.Click += BtnTargetFolder_Click;
            // 
            // txtSourceFolder
            // 
            txtSourceFolder.BackColor = SystemColors.Window;
            txtSourceFolder.Location = new Point(126, 14);
            txtSourceFolder.Name = "txtSourceFolder";
            txtSourceFolder.ReadOnly = true;
            txtSourceFolder.Size = new Size(387, 23);
            txtSourceFolder.TabIndex = 5;
            // 
            // txtTargetFolder
            // 
            txtTargetFolder.BackColor = SystemColors.Window;
            txtTargetFolder.Location = new Point(126, 45);
            txtTargetFolder.Name = "txtTargetFolder";
            txtTargetFolder.ReadOnly = true;
            txtTargetFolder.Size = new Size(387, 23);
            txtTargetFolder.TabIndex = 6;
            // 
            // backgroundWorker
            // 
            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;
            // 
            // lstLog
            // 
            lstLog.Alignment = ListViewAlignment.Left;
            lstLog.Columns.AddRange(new ColumnHeader[] { Item });
            lstLog.GridLines = true;
            lstLog.HeaderStyle = ColumnHeaderStyle.None;
            lstLog.Location = new Point(9, 130);
            lstLog.Name = "lstLog";
            lstLog.Size = new Size(504, 299);
            lstLog.TabIndex = 7;
            lstLog.UseCompatibleStateImageBehavior = false;
            lstLog.View = View.Details;
            // 
            // Item
            // 
            Item.Text = "";
            Item.Width = 500;
            // 
            // chkYearFolder
            // 
            chkYearFolder.AutoSize = true;
            chkYearFolder.Checked = true;
            chkYearFolder.CheckState = CheckState.Checked;
            chkYearFolder.Location = new Point(126, 78);
            chkYearFolder.Name = "chkYearFolder";
            chkYearFolder.Size = new Size(48, 19);
            chkYearFolder.TabIndex = 8;
            chkYearFolder.Text = "Year";
            chkYearFolder.UseVisualStyleBackColor = true;
            chkYearFolder.CheckedChanged += ChkYearFolder_CheckedChanged;
            // 
            // chkMonthFolder
            // 
            chkMonthFolder.AutoSize = true;
            chkMonthFolder.Location = new Point(177, 78);
            chkMonthFolder.Name = "chkMonthFolder";
            chkMonthFolder.Size = new Size(62, 19);
            chkMonthFolder.TabIndex = 9;
            chkMonthFolder.Text = "Month";
            chkMonthFolder.UseVisualStyleBackColor = true;
            // 
            // lblCreateFoldersFor
            // 
            lblCreateFoldersFor.AutoSize = true;
            lblCreateFoldersFor.Location = new Point(14, 79);
            lblCreateFoldersFor.Name = "lblCreateFoldersFor";
            lblCreateFoldersFor.Size = new Size(82, 15);
            lblCreateFoldersFor.TabIndex = 10;
            lblCreateFoldersFor.Text = "Create Folders";
            // 
            // lblDateSeperaotor
            // 
            lblDateSeperaotor.AutoSize = true;
            lblDateSeperaotor.Location = new Point(14, 104);
            lblDateSeperaotor.Name = "lblDateSeperaotor";
            lblDateSeperaotor.Size = new Size(103, 15);
            lblDateSeperaotor.TabIndex = 11;
            lblDateSeperaotor.Text = "Folder Name Style";
            // 
            // cmbDateSeperator
            // 
            cmbDateSeperator.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbDateSeperator.FormattingEnabled = true;
            cmbDateSeperator.Location = new Point(123, 101);
            cmbDateSeperator.MaxLength = 1;
            cmbDateSeperator.Name = "cmbDateSeperator";
            cmbDateSeperator.Size = new Size(116, 23);
            cmbDateSeperator.TabIndex = 13;
            // 
            // ImageSorterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Control;
            ClientSize = new Size(519, 441);
            Controls.Add(cmbDateSeperator);
            Controls.Add(lblDateSeperaotor);
            Controls.Add(lblCreateFoldersFor);
            Controls.Add(chkMonthFolder);
            Controls.Add(chkYearFolder);
            Controls.Add(lstLog);
            Controls.Add(txtTargetFolder);
            Controls.Add(txtSourceFolder);
            Controls.Add(btnTargetFolder);
            Controls.Add(btnSourceFolder);
            Controls.Add(btnProcess);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ImageSorterForm";
            Text = "Image File Sorter";
            ResumeLayout(false);
            PerformLayout();
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