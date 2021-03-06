﻿namespace SampleApp {
   partial class FormCreateLiveEvent {
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
         this.ctrlOptionsGroup = new System.Windows.Forms.GroupBox();
         this.label5 = new System.Windows.Forms.Label();
         this.ctrlSource = new System.Windows.Forms.ComboBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlVideoStereoscopyType = new System.Windows.Forms.ComboBox();
         this.ctrlDescription = new System.Windows.Forms.TextBox();
         this.label4 = new System.Windows.Forms.Label();
         this.ctrlTitle = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlPermission = new System.Windows.Forms.ComboBox();
         this.ctrlActionsGroup = new System.Windows.Forms.GroupBox();
         this.ctrlCreate = new System.Windows.Forms.Button();
         this.ctrlStatus = new System.Windows.Forms.Label();
         this.ctrlStatusGroup = new System.Windows.Forms.GroupBox();
         this.label6 = new System.Windows.Forms.Label();
         this.ctrlCopyRTMPUrlToCB = new System.Windows.Forms.Button();
         this.ctrlRTMPUrl = new System.Windows.Forms.TextBox();
         this.ctrlOptionsGroup.SuspendLayout();
         this.ctrlActionsGroup.SuspendLayout();
         this.ctrlStatusGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlOptionsGroup
         // 
         this.ctrlOptionsGroup.Controls.Add(this.label5);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlSource);
         this.ctrlOptionsGroup.Controls.Add(this.label2);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlVideoStereoscopyType);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlDescription);
         this.ctrlOptionsGroup.Controls.Add(this.label4);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlTitle);
         this.ctrlOptionsGroup.Controls.Add(this.label3);
         this.ctrlOptionsGroup.Controls.Add(this.label1);
         this.ctrlOptionsGroup.Controls.Add(this.ctrlPermission);
         this.ctrlOptionsGroup.Location = new System.Drawing.Point(14, 3);
         this.ctrlOptionsGroup.Name = "ctrlOptionsGroup";
         this.ctrlOptionsGroup.Size = new System.Drawing.Size(322, 279);
         this.ctrlOptionsGroup.TabIndex = 2;
         this.ctrlOptionsGroup.TabStop = false;
         this.ctrlOptionsGroup.Text = "Options";
         // 
         // label5
         // 
         this.label5.AutoSize = true;
         this.label5.Location = new System.Drawing.Point(17, 228);
         this.label5.Name = "label5";
         this.label5.Size = new System.Drawing.Size(41, 13);
         this.label5.TabIndex = 12;
         this.label5.Text = "Source";
         // 
         // ctrlSource
         // 
         this.ctrlSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.ctrlSource.Items.AddRange(new object[] {
            "RTMP",
            "SEGMENTED_TS",
            "SEGMENTED_MP4"});
         this.ctrlSource.Location = new System.Drawing.Point(20, 246);
         this.ctrlSource.Name = "ctrlSource";
         this.ctrlSource.Size = new System.Drawing.Size(283, 21);
         this.ctrlSource.TabIndex = 4;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(17, 174);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(89, 13);
         this.label2.TabIndex = 10;
         this.label2.Text = "Stereoscopy type";
         // 
         // ctrlVideoStereoscopyType
         // 
         this.ctrlVideoStereoscopyType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.ctrlVideoStereoscopyType.Items.AddRange(new object[] {
            "DEFAULT",
            "MONOSCOPIC",
            "TOP_BOTTOM_STEREOSCOPIC",
            "LEFT_RIGHT_STEREOSCOPIC",
            "DUAL_FISHEYE"});
         this.ctrlVideoStereoscopyType.Location = new System.Drawing.Point(20, 192);
         this.ctrlVideoStereoscopyType.Name = "ctrlVideoStereoscopyType";
         this.ctrlVideoStereoscopyType.Size = new System.Drawing.Size(283, 21);
         this.ctrlVideoStereoscopyType.TabIndex = 3;
         // 
         // ctrlDescription
         // 
         this.ctrlDescription.Location = new System.Drawing.Point(15, 88);
         this.ctrlDescription.Multiline = true;
         this.ctrlDescription.Name = "ctrlDescription";
         this.ctrlDescription.Size = new System.Drawing.Size(287, 20);
         this.ctrlDescription.TabIndex = 1;
         // 
         // label4
         // 
         this.label4.AutoSize = true;
         this.label4.Location = new System.Drawing.Point(15, 71);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(60, 13);
         this.label4.TabIndex = 7;
         this.label4.Text = "Description";
         // 
         // ctrlTitle
         // 
         this.ctrlTitle.Location = new System.Drawing.Point(15, 39);
         this.ctrlTitle.Multiline = true;
         this.ctrlTitle.Name = "ctrlTitle";
         this.ctrlTitle.Size = new System.Drawing.Size(287, 20);
         this.ctrlTitle.TabIndex = 0;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(15, 23);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(27, 13);
         this.label3.TabIndex = 5;
         this.label3.Text = "Title";
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(15, 122);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(57, 13);
         this.label1.TabIndex = 1;
         this.label1.Text = "Permission";
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
         this.ctrlPermission.Location = new System.Drawing.Point(20, 140);
         this.ctrlPermission.Name = "ctrlPermission";
         this.ctrlPermission.Size = new System.Drawing.Size(283, 21);
         this.ctrlPermission.TabIndex = 2;
         // 
         // ctrlActionsGroup
         // 
         this.ctrlActionsGroup.Controls.Add(this.ctrlCreate);
         this.ctrlActionsGroup.Location = new System.Drawing.Point(14, 288);
         this.ctrlActionsGroup.Name = "ctrlActionsGroup";
         this.ctrlActionsGroup.Size = new System.Drawing.Size(322, 54);
         this.ctrlActionsGroup.TabIndex = 4;
         this.ctrlActionsGroup.TabStop = false;
         this.ctrlActionsGroup.Text = "Actions";
         // 
         // ctrlCreate
         // 
         this.ctrlCreate.Location = new System.Drawing.Point(18, 19);
         this.ctrlCreate.Name = "ctrlCreate";
         this.ctrlCreate.Size = new System.Drawing.Size(65, 23);
         this.ctrlCreate.TabIndex = 5;
         this.ctrlCreate.Text = "Create";
         this.ctrlCreate.UseVisualStyleBackColor = true;
         this.ctrlCreate.Click += new System.EventHandler(this.ctrlCreate_Click);
         // 
         // ctrlStatus
         // 
         this.ctrlStatus.ForeColor = System.Drawing.Color.Red;
         this.ctrlStatus.Location = new System.Drawing.Point(17, 16);
         this.ctrlStatus.Name = "ctrlStatus";
         this.ctrlStatus.Size = new System.Drawing.Size(287, 23);
         this.ctrlStatus.TabIndex = 2;
         this.ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         // 
         // ctrlStatusGroup
         // 
         this.ctrlStatusGroup.Controls.Add(this.ctrlRTMPUrl);
         this.ctrlStatusGroup.Controls.Add(this.ctrlCopyRTMPUrlToCB);
         this.ctrlStatusGroup.Controls.Add(this.label6);
         this.ctrlStatusGroup.Controls.Add(this.ctrlStatus);
         this.ctrlStatusGroup.Location = new System.Drawing.Point(14, 348);
         this.ctrlStatusGroup.Name = "ctrlStatusGroup";
         this.ctrlStatusGroup.Size = new System.Drawing.Size(322, 118);
         this.ctrlStatusGroup.TabIndex = 3;
         this.ctrlStatusGroup.TabStop = false;
         this.ctrlStatusGroup.Text = "Status";
         // 
         // label6
         // 
         this.label6.AutoSize = true;
         this.label6.Location = new System.Drawing.Point(13, 49);
         this.label6.Name = "label6";
         this.label6.Size = new System.Drawing.Size(54, 13);
         this.label6.TabIndex = 3;
         this.label6.Text = "RTMP Url";
         // 
         // ctrlCopyRTMPUrlToCB
         // 
         this.ctrlCopyRTMPUrlToCB.Location = new System.Drawing.Point(131, 89);
         this.ctrlCopyRTMPUrlToCB.Name = "ctrlCopyRTMPUrlToCB";
         this.ctrlCopyRTMPUrlToCB.Size = new System.Drawing.Size(171, 23);
         this.ctrlCopyRTMPUrlToCB.TabIndex = 6;
         this.ctrlCopyRTMPUrlToCB.Text = "Copy RTMP Url To Clipboard";
         this.ctrlCopyRTMPUrlToCB.UseVisualStyleBackColor = true;
         this.ctrlCopyRTMPUrlToCB.Click += new System.EventHandler(this.ctrlCopyRTMPUrlToCB_Click);
         // 
         // ctrlRTMPUrl
         // 
         this.ctrlRTMPUrl.Location = new System.Drawing.Point(15, 66);
         this.ctrlRTMPUrl.Name = "ctrlRTMPUrl";
         this.ctrlRTMPUrl.ReadOnly = true;
         this.ctrlRTMPUrl.Size = new System.Drawing.Size(287, 20);
         this.ctrlRTMPUrl.TabIndex = 6;
         // 
         // FormCreateLiveEvent
         // 
         this.Controls.Add(this.ctrlActionsGroup);
         this.Controls.Add(this.ctrlStatusGroup);
         this.Controls.Add(this.ctrlOptionsGroup);
         this.Name = "FormCreateLiveEvent";
         this.Size = new System.Drawing.Size(350, 480);
         this.ctrlOptionsGroup.ResumeLayout(false);
         this.ctrlOptionsGroup.PerformLayout();
         this.ctrlActionsGroup.ResumeLayout(false);
         this.ctrlStatusGroup.ResumeLayout(false);
         this.ctrlStatusGroup.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox ctrlOptionsGroup;
      private System.Windows.Forms.Label label5;
      private System.Windows.Forms.ComboBox ctrlSource;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.ComboBox ctrlVideoStereoscopyType;
      private System.Windows.Forms.TextBox ctrlDescription;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.TextBox ctrlTitle;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.ComboBox ctrlPermission;
      private System.Windows.Forms.GroupBox ctrlActionsGroup;
      private System.Windows.Forms.Button ctrlCreate;
      private System.Windows.Forms.Label ctrlStatus;
      private System.Windows.Forms.GroupBox ctrlStatusGroup;
      private System.Windows.Forms.Label label6;
      private System.Windows.Forms.Button ctrlCopyRTMPUrlToCB;
      private System.Windows.Forms.TextBox ctrlRTMPUrl;
   }
}
