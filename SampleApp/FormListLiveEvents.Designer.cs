namespace SampleApp {
   partial class FormListLiveEvents {
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
         this.ctrlQuery = new System.Windows.Forms.Button();
         this.ctrlStatus = new System.Windows.Forms.Label();
         this.ctrlEventsList = new System.Windows.Forms.ListBox();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlDelete = new System.Windows.Forms.Button();
         this.ctrlFinish = new System.Windows.Forms.Button();
         this.ctrlRefresh = new System.Windows.Forms.Button();
         this.ctrlLiveEventDetail = new System.Windows.Forms.ListBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.ctrlSelectFile = new System.Windows.Forms.Button();
         this.ctrlSelectedFile = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlUpload = new System.Windows.Forms.Button();
         this.ctrlVideoChooser = new System.Windows.Forms.OpenFileDialog();
         this.ctrlRawProgress = new System.Windows.Forms.Label();
         this.ctrlProgressBar = new System.Windows.Forms.ProgressBar();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlQuery
         // 
         this.ctrlQuery.Location = new System.Drawing.Point(14, 13);
         this.ctrlQuery.Name = "ctrlQuery";
         this.ctrlQuery.Size = new System.Drawing.Size(322, 23);
         this.ctrlQuery.TabIndex = 0;
         this.ctrlQuery.Text = "List all";
         this.ctrlQuery.UseVisualStyleBackColor = true;
         this.ctrlQuery.Click += new System.EventHandler(this.ctrlQuery_Click);
         // 
         // ctrlStatus
         // 
         this.ctrlStatus.Location = new System.Drawing.Point(11, 39);
         this.ctrlStatus.Name = "ctrlStatus";
         this.ctrlStatus.Size = new System.Drawing.Size(325, 23);
         this.ctrlStatus.TabIndex = 2;
         this.ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlEventsList
         // 
         this.ctrlEventsList.FormattingEnabled = true;
         this.ctrlEventsList.Location = new System.Drawing.Point(14, 65);
         this.ctrlEventsList.Name = "ctrlEventsList";
         this.ctrlEventsList.Size = new System.Drawing.Size(322, 69);
         this.ctrlEventsList.TabIndex = 3;
         this.ctrlEventsList.SelectedIndexChanged += new System.EventHandler(this.ctrlEventsList_SelectedIndexChanged);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlDelete);
         this.groupBox1.Controls.Add(this.ctrlFinish);
         this.groupBox1.Controls.Add(this.ctrlRefresh);
         this.groupBox1.Controls.Add(this.ctrlLiveEventDetail);
         this.groupBox1.Location = new System.Drawing.Point(14, 140);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(322, 165);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Event Ops";
         // 
         // ctrlDelete
         // 
         this.ctrlDelete.Location = new System.Drawing.Point(169, 136);
         this.ctrlDelete.Name = "ctrlDelete";
         this.ctrlDelete.Size = new System.Drawing.Size(75, 23);
         this.ctrlDelete.TabIndex = 3;
         this.ctrlDelete.Text = "Delete";
         this.ctrlDelete.UseVisualStyleBackColor = true;
         this.ctrlDelete.Click += new System.EventHandler(this.ctrlDelete_Click);
         // 
         // ctrlFinish
         // 
         this.ctrlFinish.Location = new System.Drawing.Point(88, 136);
         this.ctrlFinish.Name = "ctrlFinish";
         this.ctrlFinish.Size = new System.Drawing.Size(75, 23);
         this.ctrlFinish.TabIndex = 2;
         this.ctrlFinish.Text = "Finish";
         this.ctrlFinish.UseVisualStyleBackColor = true;
         this.ctrlFinish.Click += new System.EventHandler(this.ctrlFinish_Click);
         // 
         // ctrlRefresh
         // 
         this.ctrlRefresh.Location = new System.Drawing.Point(7, 136);
         this.ctrlRefresh.Name = "ctrlRefresh";
         this.ctrlRefresh.Size = new System.Drawing.Size(75, 23);
         this.ctrlRefresh.TabIndex = 1;
         this.ctrlRefresh.Text = "Refresh";
         this.ctrlRefresh.UseVisualStyleBackColor = true;
         this.ctrlRefresh.Click += new System.EventHandler(this.ctrlRefresh_Click);
         // 
         // ctrlLiveEventDetail
         // 
         this.ctrlLiveEventDetail.FormattingEnabled = true;
         this.ctrlLiveEventDetail.Location = new System.Drawing.Point(7, 20);
         this.ctrlLiveEventDetail.Name = "ctrlLiveEventDetail";
         this.ctrlLiveEventDetail.Size = new System.Drawing.Size(309, 108);
         this.ctrlLiveEventDetail.TabIndex = 0;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.ctrlRawProgress);
         this.groupBox2.Controls.Add(this.ctrlProgressBar);
         this.groupBox2.Controls.Add(this.ctrlUpload);
         this.groupBox2.Controls.Add(this.ctrlSelectFile);
         this.groupBox2.Controls.Add(this.ctrlSelectedFile);
         this.groupBox2.Controls.Add(this.label2);
         this.groupBox2.Location = new System.Drawing.Point(14, 311);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(322, 157);
         this.groupBox2.TabIndex = 5;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Segment upload";
         // 
         // ctrlSelectFile
         // 
         this.ctrlSelectFile.Location = new System.Drawing.Point(232, 48);
         this.ctrlSelectFile.Name = "ctrlSelectFile";
         this.ctrlSelectFile.Size = new System.Drawing.Size(83, 23);
         this.ctrlSelectFile.TabIndex = 7;
         this.ctrlSelectFile.Text = "Select File";
         this.ctrlSelectFile.UseVisualStyleBackColor = true;
         this.ctrlSelectFile.Click += new System.EventHandler(this.ctrlSelectFile_Click);
         // 
         // ctrlSelectedFile
         // 
         this.ctrlSelectedFile.Location = new System.Drawing.Point(6, 48);
         this.ctrlSelectedFile.Name = "ctrlSelectedFile";
         this.ctrlSelectedFile.ReadOnly = true;
         this.ctrlSelectedFile.Size = new System.Drawing.Size(220, 20);
         this.ctrlSelectedFile.TabIndex = 6;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 32);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(23, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "File";
         // 
         // ctrlUpload
         // 
         this.ctrlUpload.Location = new System.Drawing.Point(6, 128);
         this.ctrlUpload.Name = "ctrlUpload";
         this.ctrlUpload.Size = new System.Drawing.Size(75, 23);
         this.ctrlUpload.TabIndex = 8;
         this.ctrlUpload.Text = "Upload";
         this.ctrlUpload.UseVisualStyleBackColor = true;
         this.ctrlUpload.Click += new System.EventHandler(this.ctrlUpload_Click);
         // 
         // ctrlVideoChooser
         // 
         this.ctrlVideoChooser.FileName = "openFileDialog1";
         // 
         // ctrlRawProgress
         // 
         this.ctrlRawProgress.Location = new System.Drawing.Point(6, 93);
         this.ctrlRawProgress.Name = "ctrlRawProgress";
         this.ctrlRawProgress.Size = new System.Drawing.Size(309, 20);
         this.ctrlRawProgress.TabIndex = 10;
         this.ctrlRawProgress.Text = "Progress";
         this.ctrlRawProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ctrlProgressBar
         // 
         this.ctrlProgressBar.Location = new System.Drawing.Point(6, 74);
         this.ctrlProgressBar.Name = "ctrlProgressBar";
         this.ctrlProgressBar.Size = new System.Drawing.Size(309, 16);
         this.ctrlProgressBar.TabIndex = 9;
         // 
         // FormListLiveEvents
         // 
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ctrlEventsList);
         this.Controls.Add(this.ctrlStatus);
         this.Controls.Add(this.ctrlQuery);
         this.Name = "FormListLiveEvents";
         this.Size = new System.Drawing.Size(350, 480);
         this.groupBox1.ResumeLayout(false);
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button ctrlQuery;
      private System.Windows.Forms.Label ctrlStatus;
      private System.Windows.Forms.ListBox ctrlEventsList;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.ListBox ctrlLiveEventDetail;
      private System.Windows.Forms.Button ctrlRefresh;
      private System.Windows.Forms.Button ctrlFinish;
      private System.Windows.Forms.Button ctrlDelete;
      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Button ctrlSelectFile;
      private System.Windows.Forms.TextBox ctrlSelectedFile;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button ctrlUpload;
      private System.Windows.Forms.OpenFileDialog ctrlVideoChooser;
      private System.Windows.Forms.Label ctrlRawProgress;
      private System.Windows.Forms.ProgressBar ctrlProgressBar;
   }
}
