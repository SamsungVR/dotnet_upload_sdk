﻿namespace SampleApp {
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
         this.ctrlEventsList.Size = new System.Drawing.Size(322, 95);
         this.ctrlEventsList.TabIndex = 3;
         this.ctrlEventsList.SelectedIndexChanged += new System.EventHandler(this.ctrlEventsList_SelectedIndexChanged);
         // 
         // groupBox1
         // 
         this.groupBox1.Location = new System.Drawing.Point(14, 166);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(322, 294);
         this.groupBox1.TabIndex = 4;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Event Details";
         // 
         // FormListLiveEvents
         // 
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ctrlEventsList);
         this.Controls.Add(this.ctrlStatus);
         this.Controls.Add(this.ctrlQuery);
         this.Name = "FormListLiveEvents";
         this.Size = new System.Drawing.Size(350, 480);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button ctrlQuery;
      private System.Windows.Forms.Label ctrlStatus;
      private System.Windows.Forms.ListBox ctrlEventsList;
      private System.Windows.Forms.GroupBox groupBox1;
   }
}
