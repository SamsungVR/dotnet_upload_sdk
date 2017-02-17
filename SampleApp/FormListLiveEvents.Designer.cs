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
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlDelete = new System.Windows.Forms.Button();
         this.ctrlFinish = new System.Windows.Forms.Button();
         this.ctrlRefresh = new System.Windows.Forms.Button();
         this.ctrlLiveEventDetail = new System.Windows.Forms.ListBox();
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.ctrlRawProgress = new System.Windows.Forms.Label();
         this.ctrlProgressBar = new System.Windows.Forms.ProgressBar();
         this.ctrlUpload = new System.Windows.Forms.Button();
         this.ctrlSelectFile = new System.Windows.Forms.Button();
         this.ctrlSelectedFile = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlVideoChooser = new System.Windows.Forms.OpenFileDialog();
         this.ctrlCancel = new System.Windows.Forms.Button();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.ctrlStatus = new System.Windows.Forms.Label();
         this.groupBox4 = new System.Windows.Forms.GroupBox();
         this.ctrlEventsList = new System.Windows.Forms.ListBox();
         this.groupBox1.SuspendLayout();
         this.groupBox2.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.groupBox4.SuspendLayout();
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
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlLiveEventDetail);
         this.groupBox1.Controls.Add(this.ctrlDelete);
         this.groupBox1.Controls.Add(this.ctrlFinish);
         this.groupBox1.Controls.Add(this.ctrlRefresh);
         this.groupBox1.Location = new System.Drawing.Point(14, 127);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(322, 128);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Event Ops";
         // 
         // ctrlDelete
         // 
         this.ctrlDelete.Location = new System.Drawing.Point(168, 95);
         this.ctrlDelete.Name = "ctrlDelete";
         this.ctrlDelete.Size = new System.Drawing.Size(75, 23);
         this.ctrlDelete.TabIndex = 3;
         this.ctrlDelete.Text = "Delete";
         this.ctrlDelete.UseVisualStyleBackColor = true;
         this.ctrlDelete.Click += new System.EventHandler(this.ctrlDelete_Click);
         // 
         // ctrlFinish
         // 
         this.ctrlFinish.Location = new System.Drawing.Point(87, 95);
         this.ctrlFinish.Name = "ctrlFinish";
         this.ctrlFinish.Size = new System.Drawing.Size(75, 23);
         this.ctrlFinish.TabIndex = 2;
         this.ctrlFinish.Text = "Finish";
         this.ctrlFinish.UseVisualStyleBackColor = true;
         this.ctrlFinish.Click += new System.EventHandler(this.ctrlFinish_Click);
         // 
         // ctrlRefresh
         // 
         this.ctrlRefresh.Location = new System.Drawing.Point(6, 95);
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
         this.ctrlLiveEventDetail.Size = new System.Drawing.Size(309, 69);
         this.ctrlLiveEventDetail.TabIndex = 0;
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.ctrlCancel);
         this.groupBox2.Controls.Add(this.ctrlRawProgress);
         this.groupBox2.Controls.Add(this.ctrlProgressBar);
         this.groupBox2.Controls.Add(this.ctrlUpload);
         this.groupBox2.Controls.Add(this.ctrlSelectFile);
         this.groupBox2.Controls.Add(this.ctrlSelectedFile);
         this.groupBox2.Controls.Add(this.label2);
         this.groupBox2.Location = new System.Drawing.Point(15, 261);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(322, 148);
         this.groupBox2.TabIndex = 5;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Segment upload";
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
         // ctrlUpload
         // 
         this.ctrlUpload.Location = new System.Drawing.Point(6, 116);
         this.ctrlUpload.Name = "ctrlUpload";
         this.ctrlUpload.Size = new System.Drawing.Size(75, 23);
         this.ctrlUpload.TabIndex = 8;
         this.ctrlUpload.Text = "Upload";
         this.ctrlUpload.UseVisualStyleBackColor = true;
         this.ctrlUpload.Click += new System.EventHandler(this.ctrlUpload_Click);
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
         // ctrlVideoChooser
         // 
         this.ctrlVideoChooser.FileName = "openFileDialog1";
         // 
         // ctrlCancel
         // 
         this.ctrlCancel.Location = new System.Drawing.Point(87, 116);
         this.ctrlCancel.Name = "ctrlCancel";
         this.ctrlCancel.Size = new System.Drawing.Size(67, 23);
         this.ctrlCancel.TabIndex = 11;
         this.ctrlCancel.Text = "Cancel";
         this.ctrlCancel.UseVisualStyleBackColor = true;
         this.ctrlCancel.Click += new System.EventHandler(this.ctrlCancel_Click);
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.ctrlStatus);
         this.groupBox3.Location = new System.Drawing.Point(15, 415);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(320, 54);
         this.groupBox3.TabIndex = 6;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Status";
         // 
         // ctrlStatus
         // 
         this.ctrlStatus.ForeColor = System.Drawing.Color.Red;
         this.ctrlStatus.Location = new System.Drawing.Point(14, 16);
         this.ctrlStatus.Name = "ctrlStatus";
         this.ctrlStatus.Size = new System.Drawing.Size(300, 23);
         this.ctrlStatus.TabIndex = 3;
         this.ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // groupBox4
         // 
         this.groupBox4.Controls.Add(this.ctrlEventsList);
         this.groupBox4.Location = new System.Drawing.Point(14, 43);
         this.groupBox4.Name = "groupBox4";
         this.groupBox4.Size = new System.Drawing.Size(321, 85);
         this.groupBox4.TabIndex = 7;
         this.groupBox4.TabStop = false;
         this.groupBox4.Text = "Available Events";
         // 
         // ctrlEventsList
         // 
         this.ctrlEventsList.FormattingEnabled = true;
         this.ctrlEventsList.Location = new System.Drawing.Point(10, 19);
         this.ctrlEventsList.Name = "ctrlEventsList";
         this.ctrlEventsList.Size = new System.Drawing.Size(305, 56);
         this.ctrlEventsList.TabIndex = 4;
         this.ctrlEventsList.SelectedIndexChanged += new System.EventHandler(this.ctrlEventsList_SelectedIndexChanged);
         // 
         // FormListLiveEvents
         // 
         this.Controls.Add(this.groupBox4);
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ctrlQuery);
         this.Name = "FormListLiveEvents";
         this.Size = new System.Drawing.Size(350, 480);
         this.groupBox1.ResumeLayout(false);
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.groupBox3.ResumeLayout(false);
         this.groupBox4.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button ctrlQuery;
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
      private System.Windows.Forms.Button ctrlCancel;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label ctrlStatus;
      private System.Windows.Forms.GroupBox groupBox4;
      private System.Windows.Forms.ListBox ctrlEventsList;
   }
}
