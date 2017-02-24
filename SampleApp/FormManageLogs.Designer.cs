namespace SampleApp {
	
   partial class FormManageLogs {
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
         this.groupBox2 = new System.Windows.Forms.GroupBox();
         this.button1 = new System.Windows.Forms.Button();
         this.ctrlSelectFile = new System.Windows.Forms.Button();
         this.ctrlSelectedFile = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlPreview = new System.Windows.Forms.Button();
         this.ctrlClear = new System.Windows.Forms.Button();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlLogFileChooser = new System.Windows.Forms.SaveFileDialog();
         this.groupBox3 = new System.Windows.Forms.GroupBox();
         this.ctrlStatus = new System.Windows.Forms.Label();
         this.groupBox2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.groupBox3.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBox2
         // 
         this.groupBox2.Controls.Add(this.button1);
         this.groupBox2.Controls.Add(this.ctrlSelectFile);
         this.groupBox2.Controls.Add(this.ctrlSelectedFile);
         this.groupBox2.Controls.Add(this.label2);
         this.groupBox2.Location = new System.Drawing.Point(13, 14);
         this.groupBox2.Name = "groupBox2";
         this.groupBox2.Size = new System.Drawing.Size(322, 107);
         this.groupBox2.TabIndex = 5;
         this.groupBox2.TabStop = false;
         this.groupBox2.Text = "Log file";
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(98, 64);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(157, 23);
         this.button1.TabIndex = 8;
         this.button1.Text = "Copy filename to Clipboard";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // ctrlSelectFile
         // 
         this.ctrlSelectFile.Location = new System.Drawing.Point(9, 64);
         this.ctrlSelectFile.Name = "ctrlSelectFile";
         this.ctrlSelectFile.Size = new System.Drawing.Size(83, 23);
         this.ctrlSelectFile.TabIndex = 7;
         this.ctrlSelectFile.Text = "Select File";
         this.ctrlSelectFile.UseVisualStyleBackColor = true;
         this.ctrlSelectFile.Click += new System.EventHandler(this.ctrlSelectFile_Click);
         // 
         // ctrlSelectedFile
         // 
         this.ctrlSelectedFile.Location = new System.Drawing.Point(9, 38);
         this.ctrlSelectedFile.Name = "ctrlSelectedFile";
         this.ctrlSelectedFile.ReadOnly = true;
         this.ctrlSelectedFile.Size = new System.Drawing.Size(302, 20);
         this.ctrlSelectedFile.TabIndex = 6;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 22);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(23, 13);
         this.label2.TabIndex = 5;
         this.label2.Text = "File";
         // 
         // ctrlPreview
         // 
         this.ctrlPreview.Location = new System.Drawing.Point(6, 19);
         this.ctrlPreview.Name = "ctrlPreview";
         this.ctrlPreview.Size = new System.Drawing.Size(197, 23);
         this.ctrlPreview.TabIndex = 1;
         this.ctrlPreview.Text = "Preview log data in External App";
         this.ctrlPreview.UseVisualStyleBackColor = true;
         this.ctrlPreview.Click += new System.EventHandler(this.ctrlPreview_Click);
         // 
         // ctrlClear
         // 
         this.ctrlClear.Location = new System.Drawing.Point(209, 19);
         this.ctrlClear.Name = "ctrlClear";
         this.ctrlClear.Size = new System.Drawing.Size(75, 23);
         this.ctrlClear.TabIndex = 2;
         this.ctrlClear.Text = "Clear log file";
         this.ctrlClear.UseVisualStyleBackColor = true;
         this.ctrlClear.Click += new System.EventHandler(this.ctrlClear_Click);
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlClear);
         this.groupBox1.Controls.Add(this.ctrlPreview);
         this.groupBox1.Location = new System.Drawing.Point(14, 127);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(322, 55);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Actions";
         // 
         // ctrlLogFileChooser
         // 
         this.ctrlLogFileChooser.CreatePrompt = true;
         this.ctrlLogFileChooser.FileName = "vrsdk.log";
         this.ctrlLogFileChooser.Title = "Choose log file";
         // 
         // groupBox3
         // 
         this.groupBox3.Controls.Add(this.ctrlStatus);
         this.groupBox3.Location = new System.Drawing.Point(13, 188);
         this.groupBox3.Name = "groupBox3";
         this.groupBox3.Size = new System.Drawing.Size(320, 86);
         this.groupBox3.TabIndex = 7;
         this.groupBox3.TabStop = false;
         this.groupBox3.Text = "Status";
         // 
         // ctrlStatus
         // 
         this.ctrlStatus.ForeColor = System.Drawing.Color.Red;
         this.ctrlStatus.Location = new System.Drawing.Point(14, 16);
         this.ctrlStatus.Name = "ctrlStatus";
         this.ctrlStatus.Size = new System.Drawing.Size(300, 59);
         this.ctrlStatus.TabIndex = 3;
         this.ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // FormManageLogs
         // 
         this.Controls.Add(this.groupBox3);
         this.Controls.Add(this.groupBox2);
         this.Controls.Add(this.groupBox1);
         this.Name = "FormManageLogs";
         this.Size = new System.Drawing.Size(350, 287);
         this.Load += new System.EventHandler(this.FormManageLogs_Load);
         this.groupBox2.ResumeLayout(false);
         this.groupBox2.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox3.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox2;
      private System.Windows.Forms.Button ctrlSelectFile;
      private System.Windows.Forms.TextBox ctrlSelectedFile;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button ctrlPreview;
      private System.Windows.Forms.Button ctrlClear;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button button1;
      private System.Windows.Forms.SaveFileDialog ctrlLogFileChooser;
      private System.Windows.Forms.GroupBox groupBox3;
      private System.Windows.Forms.Label ctrlStatus;
   }
}
