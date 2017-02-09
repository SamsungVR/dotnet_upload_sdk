namespace SampleApp {
   partial class FormUploadVideo {
      /// <summary> 
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary> 
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing) {
         if (disposing && (components != null)) {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Component Designer generated code

      /// <summary> 
      /// Required method for Designer support - do not modify 
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent() {
         this.ctrlVideoChooser = new System.Windows.Forms.OpenFileDialog();
         this.ctrlStatusGroup = new System.Windows.Forms.GroupBox();
         this.ctrlUploadStatus = new System.Windows.Forms.Label();
         this.ctrlRawProgress = new System.Windows.Forms.Label();
         this.ctrlProgressBar = new System.Windows.Forms.ProgressBar();
         this.ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
         this.ctrlDescription = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.ctrlTitle = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ctrlSelectFile = new System.Windows.Forms.Button();
         this.ctrlSelectedFile = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlPermission = new System.Windows.Forms.ComboBox();
         this.ctrlActionsGroup = new System.Windows.Forms.GroupBox();
         this.ctrlCancel = new System.Windows.Forms.Button();
         this.ctrlRetry = new System.Windows.Forms.Button();
         this.ctrlUpload = new System.Windows.Forms.Button();
         this.ctrlStatusGroup.SuspendLayout();
         this.ctrlOptionsGroup.SuspendLayout();
         this.ctrlActionsGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlVideoChooser
         // 
         this.ctrlVideoChooser.FileName = "openFileDialog1";
         // 
         // ctrlStatusGroup
         // 
         this.ctrlStatusGroup.Controls.Add(this.ctrlUploadStatus);
         this.ctrlStatusGroup.Controls.Add(this.ctrlRawProgress);
         this.ctrlStatusGroup.Controls.Add(this.ctrlProgressBar);
         this.ctrlStatusGroup.Location = new System.Drawing.Point(13, 333);
         this.ctrlStatusGroup.Name = "ctrlStatusGroup";
         this.ctrlStatusGroup.Size = new System.Drawing.Size(322, 131);
         this.ctrlStatusGroup.TabIndex = 0;
         this.ctrlStatusGroup.TabStop = false;
         this.ctrlStatusGroup.Text = "Status";
         // 
         // ctrlUploadStatus
         // 
         this.ctrlUploadStatus.Location = new System.Drawing.Point(18, 90);
         this.ctrlUploadStatus.Name = "ctrlUploadStatus";
         this.ctrlUploadStatus.Size = new System.Drawing.Size(287, 23);
         this.ctrlUploadStatus.TabIndex = 2;
         this.ctrlUploadStatus.Text = "label1";
         this.ctrlUploadStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlUploadStatus.Click += new System.EventHandler(this.ctrlUploadStatus_Click);
         // 
         // ctrlRawProgress
         // 
         this.ctrlRawProgress.Location = new System.Drawing.Point(111, 55);
         this.ctrlRawProgress.Name = "ctrlRawProgress";
         this.ctrlRawProgress.Size = new System.Drawing.Size(100, 23);
         this.ctrlRawProgress.TabIndex = 1;
         this.ctrlRawProgress.Text = "Progress";
         this.ctrlRawProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ctrlProgressBar
         // 
         this.ctrlProgressBar.Location = new System.Drawing.Point(18, 19);
         this.ctrlProgressBar.Name = "ctrlProgressBar";
         this.ctrlProgressBar.Size = new System.Drawing.Size(287, 23);
         this.ctrlProgressBar.TabIndex = 0;
         // 
         // ctrlOptionsGroup
         // 
         this.ctrlOptionsGroup.Controls.Add(this.ctrlDescription);
         this.ctrlOptionsGroup.Controls.Add(this.label4);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlTitle);
         this.ctrlOptionsGroup.Controls.Add(this.label3);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlSelectFile);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlSelectedFile);
         this.ctrlOptionsGroup.Controls.Add(this.label2);
         this.ctrlOptionsGroup.Controls.Add(this.label1);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlPermission);
         this.ctrlOptionsGroup.Location = new System.Drawing.Point(13, 14);
         this.ctrlOptionsGroup.Name = "ctrlOptionsGroup";
         this.ctrlOptionsGroup.Size = new System.Drawing.Size(322, 236);
         this.ctrlOptionsGroup.TabIndex = 1;
         this.ctrlOptionsGroup.TabStop = false;
         this.ctrlOptionsGroup.Text = "Options";
         // 
         // ctrlDescription
         // 
         this.ctrlDescription.Location = new System.Drawing.Point(18, 193);
         this.ctrlDescription.Multiline = true;
         this.ctrlDescription.Name = "ctrlDescription";
         this.ctrlDescription.Size = new System.Drawing.Size(287, 20);
         this.ctrlDescription.TabIndex = 8;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(18, 176);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(60, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Description";
         // 
         // ctrlTitle
         // 
         this.ctrlTitle.Location = new System.Drawing.Point(18, 140);
         this.ctrlTitle.Multiline = true;
         this.ctrlTitle.Name = "ctrlTitle";
         this.ctrlTitle.Size = new System.Drawing.Size(287, 20);
         this.ctrlTitle.TabIndex = 6;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(18, 124);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(27, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Title";
         // 
         // ctrlSelectFile
         // 
         this.ctrlSelectFile.Location = new System.Drawing.Point(222, 89);
         this.ctrlSelectFile.Name = "ctrlSelectFile";
         this.ctrlSelectFile.Size = new System.Drawing.Size(83, 23);
         this.ctrlSelectFile.TabIndex = 4;
         this.ctrlSelectFile.Text = "Select File";
         this.ctrlSelectFile.UseVisualStyleBackColor = true;
         this.ctrlSelectFile.Click += new System.EventHandler(this.ctrlSelectFile_Click);
         // 
         // ctrlSelectedFile
         // 
         this.ctrlSelectedFile.Location = new System.Drawing.Point(18, 89);
         this.ctrlSelectedFile.Name = "ctrlSelectedFile";
         this.ctrlSelectedFile.ReadOnly = true;
         this.ctrlSelectedFile.Size = new System.Drawing.Size(193, 20);
         this.ctrlSelectedFile.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(15, 73);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(23, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "File";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(15, 26);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(86, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Video permission";
         // 
         // ctrlPermission
         // 
         this.ctrlPermission.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.ctrlPermission.Items.AddRange(new object[] {
            "PRIVATE",
            "UNLISTED",
            "PUBLIC",
            "VR_ONLY",
            "WEB_ONLY"});
         this.ctrlPermission.Location = new System.Drawing.Point(18, 44);
         this.ctrlPermission.Name = "ctrlPermission";
         this.ctrlPermission.Size = new System.Drawing.Size(287, 21);
         this.ctrlPermission.TabIndex = 0;
         // 
         // ctrlActionsGroup
         // 
         this.ctrlActionsGroup.Controls.Add(this.ctrlCancel);
         this.ctrlActionsGroup.Controls.Add(this.ctrlRetry);
         this.ctrlActionsGroup.Controls.Add(this.ctrlUpload);
         this.ctrlActionsGroup.Location = new System.Drawing.Point(13, 256);
         this.ctrlActionsGroup.Name = "ctrlActionsGroup";
         this.ctrlActionsGroup.Size = new System.Drawing.Size(322, 71);
         this.ctrlActionsGroup.TabIndex = 2;
         this.ctrlActionsGroup.TabStop = false;
         this.ctrlActionsGroup.Text = "Actions";
         // 
         // ctrlCancel
         // 
         this.ctrlCancel.Location = new System.Drawing.Point(129, 34);
         this.ctrlCancel.Name = "ctrlCancel";
         this.ctrlCancel.Size = new System.Drawing.Size(67, 23);
         this.ctrlCancel.TabIndex = 2;
         this.ctrlCancel.Text = "Cancel";
         this.ctrlCancel.UseVisualStyleBackColor = true;
         this.ctrlCancel.Click += new System.EventHandler(this.ctrlCancel_Click);
         // 
         // ctrlRetry
         // 
         this.ctrlRetry.Location = new System.Drawing.Point(245, 34);
         this.ctrlRetry.Name = "ctrlRetry";
         this.ctrlRetry.Size = new System.Drawing.Size(60, 23);
         this.ctrlRetry.TabIndex = 1;
         this.ctrlRetry.Text = "Retry";
         this.ctrlRetry.UseVisualStyleBackColor = true;
         this.ctrlRetry.Click += new System.EventHandler(this.ctrlRetry_Click);
         // 
         // ctrlUpload
         // 
         this.ctrlUpload.Location = new System.Drawing.Point(18, 34);
         this.ctrlUpload.Name = "ctrlUpload";
         this.ctrlUpload.Size = new System.Drawing.Size(65, 23);
         this.ctrlUpload.TabIndex = 0;
         this.ctrlUpload.Text = "Upload";
         this.ctrlUpload.UseVisualStyleBackColor = true;
         this.ctrlUpload.Click += new System.EventHandler(this.ctrlUpload_Click);
         // 
         // FormUploadVideo
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlActionsGroup);
         this.Controls.Add(this.ctrlOptionsGroup);
         this.Controls.Add(this.ctrlStatusGroup);
         this.Name = "FormUploadVideo";
         this.Size = new System.Drawing.Size(350, 480);
         this.Load += new System.EventHandler(this.FormUploadVideo_Load);
         this.ctrlStatusGroup.ResumeLayout(false);
         this.ctrlOptionsGroup.ResumeLayout(false);
         this.ctrlOptionsGroup.PerformLayout();
         this.ctrlActionsGroup.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.OpenFileDialog ctrlVideoChooser;
      private System.Windows.Forms.GroupBox ctrlStatusGroup;
      private System.Windows.Forms.Label ctrlRawProgress;
      private System.Windows.Forms.ProgressBar ctrlProgressBar;
      private System.Windows.Forms.Label ctrlUploadStatus;
      private System.Windows.Forms.GroupBox ctrlOptionsGroup;
      private System.Windows.Forms.Button ctrlSelectFile;
      private System.Windows.Forms.TextBox ctrlSelectedFile;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox ctrlPermission;
      private System.Windows.Forms.GroupBox ctrlActionsGroup;
      private System.Windows.Forms.Button ctrlCancel;
      private System.Windows.Forms.Button ctrlRetry;
      private System.Windows.Forms.Button ctrlUpload;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ctrlDescription;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox ctrlTitle;
   }
}
