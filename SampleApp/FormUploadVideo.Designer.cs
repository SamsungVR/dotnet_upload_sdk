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
         this.ctrlEnqueueGroup = new System.Windows.Forms.GroupBox();
         this.ctrlEnqueueStatus = new System.Windows.Forms.Label();
         this.ctrlEnqueue = new System.Windows.Forms.Button();
         this.ctrlDescription = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.ctrlTitle = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ctrlSelectFile = new System.Windows.Forms.Button();
         this.ctrlSelectedFile = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlPermission = new System.Windows.Forms.ComboBox();
         this.label6 = new System.Windows.Forms.Label();
         this.ctrlRemovePending = new System.Windows.Forms.Button();
         this.ctrlPendingMoveUp = new System.Windows.Forms.Button();
         this.ctrlPendingMoveDown = new System.Windows.Forms.Button();
         this.ctrlPendingList = new System.Windows.Forms.ListBox();
         this.ctrlUploadDashboard = new System.Windows.Forms.TabControl();
         this.tabPage1 = new System.Windows.Forms.TabPage();
         this.tabPage2 = new System.Windows.Forms.TabPage();
         this.ctrlUploadProgressRaw = new System.Windows.Forms.TextBox();
         this.ctrlUploadProgressVisual = new System.Windows.Forms.ProgressBar();
         this.ctrlInProgressDetails = new System.Windows.Forms.ListBox();
         this.ctrlCancelActiveUpload = new System.Windows.Forms.Button();
         this.tabPage3 = new System.Windows.Forms.TabPage();
         this.ctrlClearCompletedList = new System.Windows.Forms.Button();
         this.ctrlCompletedList = new System.Windows.Forms.ListBox();
         this.tabPage4 = new System.Windows.Forms.TabPage();
         this.ctrlRequeueFailed = new System.Windows.Forms.Button();
         this.ctrlClearFailedList = new System.Windows.Forms.Button();
         this.ctrlFailedList = new System.Windows.Forms.ListBox();
         this.ctrlRequeueCompleted = new System.Windows.Forms.Button();
         this.ctrlEnqueueGroup.SuspendLayout();
         this.ctrlUploadDashboard.SuspendLayout();
         this.tabPage1.SuspendLayout();
         this.tabPage2.SuspendLayout();
         this.tabPage3.SuspendLayout();
         this.tabPage4.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlVideoChooser
         // 
         this.ctrlVideoChooser.FileName = "openFileDialog1";
         // 
         // ctrlEnqueueGroup
         // 
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlEnqueueStatus);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlEnqueue);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlDescription);
         this.ctrlEnqueueGroup.Controls.Add(this.label4);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlTitle);
         this.ctrlEnqueueGroup.Controls.Add(this.label3);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlSelectFile);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlSelectedFile);
         this.ctrlEnqueueGroup.Controls.Add(this.label2);
         this.ctrlEnqueueGroup.Controls.Add(this.label1);
         this.ctrlEnqueueGroup.Controls.Add(this.ctrlPermission);
         this.ctrlEnqueueGroup.Location = new System.Drawing.Point(3, 3);
         this.ctrlEnqueueGroup.Name = "ctrlEnqueueGroup";
         this.ctrlEnqueueGroup.Size = new System.Drawing.Size(359, 261);
         this.ctrlEnqueueGroup.TabIndex = 1;
         this.ctrlEnqueueGroup.TabStop = false;
         this.ctrlEnqueueGroup.Text = "Enqueue";
         // 
         // ctrlEnqueueStatus
         // 
         this.ctrlEnqueueStatus.Location = new System.Drawing.Point(6, 225);
         this.ctrlEnqueueStatus.Name = "ctrlEnqueueStatus";
         this.ctrlEnqueueStatus.Size = new System.Drawing.Size(347, 26);
         this.ctrlEnqueueStatus.TabIndex = 10;
         this.ctrlEnqueueStatus.Text = "label5";
         // 
         // ctrlEnqueue
         // 
         this.ctrlEnqueue.Location = new System.Drawing.Point(270, 188);
         this.ctrlEnqueue.Name = "ctrlEnqueue";
         this.ctrlEnqueue.Size = new System.Drawing.Size(83, 23);
         this.ctrlEnqueue.TabIndex = 9;
         this.ctrlEnqueue.Text = "Enqueue";
         this.ctrlEnqueue.UseVisualStyleBackColor = true;
         this.ctrlEnqueue.Click += new System.EventHandler(this.ctrlEnqueue_Click);
         // 
         // ctrlDescription
         // 
         this.ctrlDescription.Location = new System.Drawing.Point(6, 162);
         this.ctrlDescription.Multiline = true;
         this.ctrlDescription.Name = "ctrlDescription";
         this.ctrlDescription.Size = new System.Drawing.Size(347, 20);
         this.ctrlDescription.TabIndex = 8;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(3, 146);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(60, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Description";
         // 
         // ctrlTitle
         // 
         this.ctrlTitle.Location = new System.Drawing.Point(6, 123);
         this.ctrlTitle.Multiline = true;
         this.ctrlTitle.Name = "ctrlTitle";
         this.ctrlTitle.Size = new System.Drawing.Size(347, 20);
         this.ctrlTitle.TabIndex = 6;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(3, 107);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(27, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Title";
         // 
         // ctrlSelectFile
         // 
         this.ctrlSelectFile.Location = new System.Drawing.Point(270, 84);
         this.ctrlSelectFile.Name = "ctrlSelectFile";
         this.ctrlSelectFile.Size = new System.Drawing.Size(83, 23);
         this.ctrlSelectFile.TabIndex = 4;
         this.ctrlSelectFile.Text = "Select File";
         this.ctrlSelectFile.UseVisualStyleBackColor = true;
         this.ctrlSelectFile.Click += new System.EventHandler(this.ctrlSelectFile_Click);
         // 
         // ctrlSelectedFile
         // 
         this.ctrlSelectedFile.Location = new System.Drawing.Point(6, 84);
         this.ctrlSelectedFile.Name = "ctrlSelectedFile";
         this.ctrlSelectedFile.ReadOnly = true;
         this.ctrlSelectedFile.Size = new System.Drawing.Size(258, 20);
         this.ctrlSelectedFile.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(3, 68);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(23, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "File";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(3, 16);
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
         this.ctrlPermission.Location = new System.Drawing.Point(6, 34);
         this.ctrlPermission.Name = "ctrlPermission";
         this.ctrlPermission.Size = new System.Drawing.Size(347, 21);
         this.ctrlPermission.TabIndex = 0;
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(3, 16);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(56, 13);
         this.label6.TabIndex = 14;
         this.label6.Text = "File details";
         // 
         // ctrlRemovePending
         // 
         this.ctrlRemovePending.Location = new System.Drawing.Point(8, 185);
         this.ctrlRemovePending.Name = "ctrlRemovePending";
         this.ctrlRemovePending.Size = new System.Drawing.Size(62, 23);
         this.ctrlRemovePending.TabIndex = 16;
         this.ctrlRemovePending.Text = "Remove";
         this.ctrlRemovePending.UseVisualStyleBackColor = true;
         this.ctrlRemovePending.Click += new System.EventHandler(this.ctrlRemovePending_Click);
         // 
         // ctrlPendingMoveUp
         // 
         this.ctrlPendingMoveUp.Location = new System.Drawing.Point(245, 185);
         this.ctrlPendingMoveUp.Name = "ctrlPendingMoveUp";
         this.ctrlPendingMoveUp.Size = new System.Drawing.Size(50, 23);
         this.ctrlPendingMoveUp.TabIndex = 15;
         this.ctrlPendingMoveUp.Text = "Up";
         this.ctrlPendingMoveUp.UseVisualStyleBackColor = true;
         this.ctrlPendingMoveUp.Click += new System.EventHandler(this.ctrlPendingMoveUp_Click);
         // 
         // ctrlPendingMoveDown
         // 
         this.ctrlPendingMoveDown.Location = new System.Drawing.Point(301, 185);
         this.ctrlPendingMoveDown.Name = "ctrlPendingMoveDown";
         this.ctrlPendingMoveDown.Size = new System.Drawing.Size(48, 23);
         this.ctrlPendingMoveDown.TabIndex = 13;
         this.ctrlPendingMoveDown.Text = "Down";
         this.ctrlPendingMoveDown.UseVisualStyleBackColor = true;
         this.ctrlPendingMoveDown.Click += new System.EventHandler(this.ctrlPendingMoveDown_Click);
         // 
         // ctrlPendingList
         // 
         this.ctrlPendingList.FormattingEnabled = true;
         this.ctrlPendingList.HorizontalScrollbar = true;
         this.ctrlPendingList.Location = new System.Drawing.Point(8, 6);
         this.ctrlPendingList.Name = "ctrlPendingList";
         this.ctrlPendingList.ScrollAlwaysVisible = true;
         this.ctrlPendingList.Size = new System.Drawing.Size(341, 173);
         this.ctrlPendingList.TabIndex = 14;
         // 
         // ctrlUploadDashboard
         // 
         this.ctrlUploadDashboard.Controls.Add(this.tabPage1);
         this.ctrlUploadDashboard.Controls.Add(this.tabPage2);
         this.ctrlUploadDashboard.Controls.Add(this.tabPage3);
         this.ctrlUploadDashboard.Controls.Add(this.tabPage4);
         this.ctrlUploadDashboard.Location = new System.Drawing.Point(3, 270);
         this.ctrlUploadDashboard.Name = "ctrlUploadDashboard";
         this.ctrlUploadDashboard.SelectedIndex = 0;
         this.ctrlUploadDashboard.Size = new System.Drawing.Size(363, 238);
         this.ctrlUploadDashboard.TabIndex = 2;
         // 
         // tabPage1
         // 
         this.tabPage1.Controls.Add(this.ctrlPendingList);
         this.tabPage1.Controls.Add(this.ctrlRemovePending);
         this.tabPage1.Controls.Add(this.ctrlPendingMoveUp);
         this.tabPage1.Controls.Add(this.ctrlPendingMoveDown);
         this.tabPage1.Location = new System.Drawing.Point(4, 22);
         this.tabPage1.Name = "tabPage1";
         this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage1.Size = new System.Drawing.Size(355, 212);
         this.tabPage1.TabIndex = 0;
         this.tabPage1.Text = "Pending";
         this.tabPage1.UseVisualStyleBackColor = true;
         // 
         // tabPage2
         // 
         this.tabPage2.Controls.Add(this.ctrlUploadProgressRaw);
         this.tabPage2.Controls.Add(this.ctrlUploadProgressVisual);
         this.tabPage2.Controls.Add(this.ctrlInProgressDetails);
         this.tabPage2.Controls.Add(this.ctrlCancelActiveUpload);
         this.tabPage2.Controls.Add(this.label6);
         this.tabPage2.Location = new System.Drawing.Point(4, 22);
         this.tabPage2.Name = "tabPage2";
         this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
         this.tabPage2.Size = new System.Drawing.Size(355, 212);
         this.tabPage2.TabIndex = 1;
         this.tabPage2.Text = "In Progress";
         this.tabPage2.UseVisualStyleBackColor = true;
         // 
         // ctrlUploadProgressRaw
         // 
         this.ctrlUploadProgressRaw.Location = new System.Drawing.Point(6, 160);
         this.ctrlUploadProgressRaw.Name = "ctrlUploadProgressRaw";
         this.ctrlUploadProgressRaw.ReadOnly = true;
         this.ctrlUploadProgressRaw.Size = new System.Drawing.Size(76, 20);
         this.ctrlUploadProgressRaw.TabIndex = 19;
         // 
         // ctrlUploadProgressVisual
         // 
         this.ctrlUploadProgressVisual.Location = new System.Drawing.Point(88, 160);
         this.ctrlUploadProgressVisual.Name = "ctrlUploadProgressVisual";
         this.ctrlUploadProgressVisual.Size = new System.Drawing.Size(261, 20);
         this.ctrlUploadProgressVisual.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
         this.ctrlUploadProgressVisual.TabIndex = 18;
         // 
         // ctrlInProgressDetails
         // 
         this.ctrlInProgressDetails.FormattingEnabled = true;
         this.ctrlInProgressDetails.HorizontalScrollbar = true;
         this.ctrlInProgressDetails.Location = new System.Drawing.Point(5, 32);
         this.ctrlInProgressDetails.Name = "ctrlInProgressDetails";
         this.ctrlInProgressDetails.ScrollAlwaysVisible = true;
         this.ctrlInProgressDetails.SelectionMode = System.Windows.Forms.SelectionMode.None;
         this.ctrlInProgressDetails.Size = new System.Drawing.Size(344, 121);
         this.ctrlInProgressDetails.TabIndex = 17;
         // 
         // ctrlCancelActiveUpload
         // 
         this.ctrlCancelActiveUpload.Location = new System.Drawing.Point(227, 183);
         this.ctrlCancelActiveUpload.Name = "ctrlCancelActiveUpload";
         this.ctrlCancelActiveUpload.Size = new System.Drawing.Size(125, 23);
         this.ctrlCancelActiveUpload.TabIndex = 16;
         this.ctrlCancelActiveUpload.Text = "Cancel active upload";
         this.ctrlCancelActiveUpload.UseVisualStyleBackColor = true;
         this.ctrlCancelActiveUpload.Click += new System.EventHandler(this.ctrlCancelActiveUpload_Click);
         // 
         // tabPage3
         // 
         this.tabPage3.Controls.Add(this.ctrlRequeueCompleted);
         this.tabPage3.Controls.Add(this.ctrlClearCompletedList);
         this.tabPage3.Controls.Add(this.ctrlCompletedList);
         this.tabPage3.Location = new System.Drawing.Point(4, 22);
         this.tabPage3.Name = "tabPage3";
         this.tabPage3.Size = new System.Drawing.Size(355, 212);
         this.tabPage3.TabIndex = 2;
         this.tabPage3.Text = "Completed";
         this.tabPage3.UseVisualStyleBackColor = true;
         // 
         // ctrlClearCompletedList
         // 
         this.ctrlClearCompletedList.Location = new System.Drawing.Point(200, 185);
         this.ctrlClearCompletedList.Name = "ctrlClearCompletedList";
         this.ctrlClearCompletedList.Size = new System.Drawing.Size(60, 23);
         this.ctrlClearCompletedList.TabIndex = 17;
         this.ctrlClearCompletedList.Text = "Clear";
         this.ctrlClearCompletedList.UseVisualStyleBackColor = true;
         this.ctrlClearCompletedList.Click += new System.EventHandler(this.ctrlClearCompletedList_Click);
         // 
         // ctrlCompletedList
         // 
         this.ctrlCompletedList.FormattingEnabled = true;
         this.ctrlCompletedList.Location = new System.Drawing.Point(3, 6);
         this.ctrlCompletedList.Name = "ctrlCompletedList";
         this.ctrlCompletedList.Size = new System.Drawing.Size(346, 173);
         this.ctrlCompletedList.TabIndex = 0;
         // 
         // tabPage4
         // 
         this.tabPage4.Controls.Add(this.ctrlRequeueFailed);
         this.tabPage4.Controls.Add(this.ctrlClearFailedList);
         this.tabPage4.Controls.Add(this.ctrlFailedList);
         this.tabPage4.Location = new System.Drawing.Point(4, 22);
         this.tabPage4.Name = "tabPage4";
         this.tabPage4.Size = new System.Drawing.Size(355, 212);
         this.tabPage4.TabIndex = 3;
         this.tabPage4.Text = "Failed";
         this.tabPage4.UseVisualStyleBackColor = true;
         // 
         // ctrlRequeueFailed
         // 
         this.ctrlRequeueFailed.Location = new System.Drawing.Point(266, 182);
         this.ctrlRequeueFailed.Name = "ctrlRequeueFailed";
         this.ctrlRequeueFailed.Size = new System.Drawing.Size(83, 23);
         this.ctrlRequeueFailed.TabIndex = 20;
         this.ctrlRequeueFailed.Text = "Requeue";
         this.ctrlRequeueFailed.UseVisualStyleBackColor = true;
         this.ctrlRequeueFailed.Click += new System.EventHandler(this.ctrlRequeueFailed_Click);
         // 
         // ctrlClearFailedList
         // 
         this.ctrlClearFailedList.Location = new System.Drawing.Point(200, 182);
         this.ctrlClearFailedList.Name = "ctrlClearFailedList";
         this.ctrlClearFailedList.Size = new System.Drawing.Size(60, 23);
         this.ctrlClearFailedList.TabIndex = 19;
         this.ctrlClearFailedList.Text = "Clear";
         this.ctrlClearFailedList.UseVisualStyleBackColor = true;
         this.ctrlClearFailedList.Click += new System.EventHandler(this.ctrlClearFailedList_Click);
         // 
         // ctrlFailedList
         // 
         this.ctrlFailedList.FormattingEnabled = true;
         this.ctrlFailedList.HorizontalScrollbar = true;
         this.ctrlFailedList.Location = new System.Drawing.Point(3, 3);
         this.ctrlFailedList.Name = "ctrlFailedList";
         this.ctrlFailedList.ScrollAlwaysVisible = true;
         this.ctrlFailedList.Size = new System.Drawing.Size(346, 173);
         this.ctrlFailedList.TabIndex = 18;
         // 
         // ctrlRequeueCompleted
         // 
         this.ctrlRequeueCompleted.Location = new System.Drawing.Point(266, 185);
         this.ctrlRequeueCompleted.Name = "ctrlRequeueCompleted";
         this.ctrlRequeueCompleted.Size = new System.Drawing.Size(83, 23);
         this.ctrlRequeueCompleted.TabIndex = 18;
         this.ctrlRequeueCompleted.Text = "Requeue";
         this.ctrlRequeueCompleted.UseVisualStyleBackColor = true;
         this.ctrlRequeueCompleted.Click += new System.EventHandler(this.ctrlRequeueCompleted_Click);
         // 
         // FormUploadVideo
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlUploadDashboard);
         this.Controls.Add(this.ctrlEnqueueGroup);
         this.Name = "FormUploadVideo";
         this.Size = new System.Drawing.Size(369, 510);
         this.ctrlEnqueueGroup.ResumeLayout(false);
         this.ctrlEnqueueGroup.PerformLayout();
         this.ctrlUploadDashboard.ResumeLayout(false);
         this.tabPage1.ResumeLayout(false);
         this.tabPage2.ResumeLayout(false);
         this.tabPage2.PerformLayout();
         this.tabPage3.ResumeLayout(false);
         this.tabPage4.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.OpenFileDialog ctrlVideoChooser;
      private System.Windows.Forms.GroupBox ctrlEnqueueGroup;
      private System.Windows.Forms.Button ctrlSelectFile;
      private System.Windows.Forms.TextBox ctrlSelectedFile;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox ctrlPermission;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ctrlDescription;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox ctrlTitle;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Button ctrlRemovePending;
      private System.Windows.Forms.Button ctrlPendingMoveUp;
      private System.Windows.Forms.Button ctrlPendingMoveDown;
      private System.Windows.Forms.ListBox ctrlPendingList;
      private System.Windows.Forms.Button ctrlEnqueue;
      private System.Windows.Forms.TabControl ctrlUploadDashboard;
      private System.Windows.Forms.TabPage tabPage1;
      private System.Windows.Forms.TabPage tabPage2;
      private System.Windows.Forms.TabPage tabPage3;
      private System.Windows.Forms.TabPage tabPage4;
      private System.Windows.Forms.Button ctrlCancelActiveUpload;
      private System.Windows.Forms.Button ctrlClearCompletedList;
      private System.Windows.Forms.ListBox ctrlCompletedList;
      private System.Windows.Forms.Button ctrlClearFailedList;
      private System.Windows.Forms.ListBox ctrlFailedList;
      private System.Windows.Forms.Label ctrlEnqueueStatus;
      private System.Windows.Forms.ListBox ctrlInProgressDetails;
      private System.Windows.Forms.TextBox ctrlUploadProgressRaw;
      private System.Windows.Forms.ProgressBar ctrlUploadProgressVisual;
      private System.Windows.Forms.Button ctrlRequeueFailed;
      private System.Windows.Forms.Button ctrlRequeueCompleted;
   }
}
