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
            this.SuspendLayout();
            // 
            // btnProcess
            // 
            this.btnProcess.Location = new System.Drawing.Point(419, 13);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(93, 52);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Sort";
            this.btnProcess.UseVisualStyleBackColor = true;
            this.btnProcess.Click += new System.EventHandler(this.BtnProcess_Click);
            // 
            // btnSourceFolder
            // 
            this.btnSourceFolder.Location = new System.Drawing.Point(334, 13);
            this.btnSourceFolder.Name = "btnSourceFolder";
            this.btnSourceFolder.Size = new System.Drawing.Size(79, 23);
            this.btnSourceFolder.TabIndex = 3;
            this.btnSourceFolder.Text = "Source";
            this.btnSourceFolder.UseVisualStyleBackColor = true;
            this.btnSourceFolder.Click += new System.EventHandler(this.BtnSourceFolder_Click);
            // 
            // btnTargetFolder
            // 
            this.btnTargetFolder.Location = new System.Drawing.Point(334, 40);
            this.btnTargetFolder.Name = "btnTargetFolder";
            this.btnTargetFolder.Size = new System.Drawing.Size(79, 23);
            this.btnTargetFolder.TabIndex = 4;
            this.btnTargetFolder.Text = "Target";
            this.btnTargetFolder.UseVisualStyleBackColor = true;
            this.btnTargetFolder.Click += new System.EventHandler(this.BtnTargetFolder_Click);
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.BackColor = System.Drawing.SystemColors.Window;
            this.txtSourceFolder.Location = new System.Drawing.Point(9, 13);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.ReadOnly = true;
            this.txtSourceFolder.Size = new System.Drawing.Size(319, 23);
            this.txtSourceFolder.TabIndex = 5;
            // 
            // txtTargetFolder
            // 
            this.txtTargetFolder.BackColor = System.Drawing.SystemColors.Window;
            this.txtTargetFolder.Location = new System.Drawing.Point(9, 39);
            this.txtTargetFolder.Name = "txtTargetFolder";
            this.txtTargetFolder.ReadOnly = true;
            this.txtTargetFolder.Size = new System.Drawing.Size(319, 23);
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
            this.lstLog.Location = new System.Drawing.Point(9, 68);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(504, 361);
            this.lstLog.TabIndex = 7;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
            // 
            // Item
            // 
            this.Item.Text = "";
            this.Item.Width = 500;
            // 
            // ImageSorterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(519, 441);
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
    }
}