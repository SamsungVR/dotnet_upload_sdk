namespace SampleApp {
   partial class FormLoggedIn {
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
         this.ctrlCurrentAction = new System.Windows.Forms.Panel();
         this.panel2 = new System.Windows.Forms.Panel();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlProfilePic = new System.Windows.Forms.PictureBox();
         this.ctrlEmail = new System.Windows.Forms.Label();
         this.ctrlUsername = new System.Windows.Forms.Label();
         this.ctrlUploadVideo = new System.Windows.Forms.LinkLabel();
         this.ctrlCreateLiveEvent = new System.Windows.Forms.LinkLabel();
         this.ctrlListLiveEvents = new System.Windows.Forms.LinkLabel();
         this.panel2.SuspendLayout();
         this.groupBox1.SuspendLayout();
         ((System.ComponentModel.ISupportInitialize)(this.ctrlProfilePic)).BeginInit();
         this.SuspendLayout();
         // 
         // ctrlCurrentAction
         // 
         this.ctrlCurrentAction.BackColor = System.Drawing.SystemColors.Window;
         this.ctrlCurrentAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlCurrentAction.Location = new System.Drawing.Point(210, 4);
         this.ctrlCurrentAction.Name = "ctrlCurrentAction";
         this.ctrlCurrentAction.Size = new System.Drawing.Size(436, 501);
         this.ctrlCurrentAction.TabIndex = 1;
         // 
         // panel2
         // 
         this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.panel2.Controls.Add(this.ctrlListLiveEvents);
         this.panel2.Controls.Add(this.ctrlCreateLiveEvent);
         this.panel2.Controls.Add(this.groupBox1);
         this.panel2.Controls.Add(this.ctrlUploadVideo);
         this.panel2.Location = new System.Drawing.Point(4, 4);
         this.panel2.Name = "panel2";
         this.panel2.Size = new System.Drawing.Size(200, 501);
         this.panel2.TabIndex = 2;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlProfilePic);
         this.groupBox1.Controls.Add(this.ctrlEmail);
         this.groupBox1.Controls.Add(this.ctrlUsername);
         this.groupBox1.Location = new System.Drawing.Point(4, 4);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(191, 87);
         this.groupBox1.TabIndex = 1;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Logged in";
         // 
         // ctrlProfilePic
         // 
         this.ctrlProfilePic.Location = new System.Drawing.Point(6, 20);
         this.ctrlProfilePic.Name = "ctrlProfilePic";
         this.ctrlProfilePic.Size = new System.Drawing.Size(57, 57);
         this.ctrlProfilePic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
         this.ctrlProfilePic.TabIndex = 2;
         this.ctrlProfilePic.TabStop = false;
         // 
         // ctrlEmail
         // 
         this.ctrlEmail.Location = new System.Drawing.Point(69, 43);
         this.ctrlEmail.Name = "ctrlEmail";
         this.ctrlEmail.Size = new System.Drawing.Size(116, 34);
         this.ctrlEmail.TabIndex = 1;
         this.ctrlEmail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlUsername
         // 
         this.ctrlUsername.Location = new System.Drawing.Point(72, 20);
         this.ctrlUsername.Name = "ctrlUsername";
         this.ctrlUsername.Size = new System.Drawing.Size(113, 23);
         this.ctrlUsername.TabIndex = 0;
         this.ctrlUsername.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlUploadVideo
         // 
         this.ctrlUploadVideo.AutoSize = true;
         this.ctrlUploadVideo.Location = new System.Drawing.Point(7, 104);
         this.ctrlUploadVideo.Name = "ctrlUploadVideo";
         this.ctrlUploadVideo.Size = new System.Drawing.Size(70, 13);
         this.ctrlUploadVideo.TabIndex = 0;
         this.ctrlUploadVideo.TabStop = true;
         this.ctrlUploadVideo.Text = "Upload video";
         this.ctrlUploadVideo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ctrlUploadVideo_LinkClicked);
         // 
         // ctrlCreateLiveEvent
         // 
         this.ctrlCreateLiveEvent.AutoSize = true;
         this.ctrlCreateLiveEvent.Location = new System.Drawing.Point(7, 132);
         this.ctrlCreateLiveEvent.Name = "ctrlCreateLiveEvent";
         this.ctrlCreateLiveEvent.Size = new System.Drawing.Size(87, 13);
         this.ctrlCreateLiveEvent.TabIndex = 2;
         this.ctrlCreateLiveEvent.TabStop = true;
         this.ctrlCreateLiveEvent.Text = "Create live event";
         this.ctrlCreateLiveEvent.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ctrlCreateLiveEvent_LinkClicked);
         // 
         // ctrlListLiveEvents
         // 
         this.ctrlListLiveEvents.AutoSize = true;
         this.ctrlListLiveEvents.Location = new System.Drawing.Point(7, 160);
         this.ctrlListLiveEvents.Name = "ctrlListLiveEvents";
         this.ctrlListLiveEvents.Size = new System.Drawing.Size(77, 13);
         this.ctrlListLiveEvents.TabIndex = 3;
         this.ctrlListLiveEvents.TabStop = true;
         this.ctrlListLiveEvents.Text = "List live events";
         // 
         // FormLoggedIn
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.panel2);
         this.Controls.Add(this.ctrlCurrentAction);
         this.Name = "FormLoggedIn";
         this.Size = new System.Drawing.Size(649, 508);
         this.Load += new System.EventHandler(this.FormLoggedIn_Load);
         this.panel2.ResumeLayout(false);
         this.panel2.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         ((System.ComponentModel.ISupportInitialize)(this.ctrlProfilePic)).EndInit();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Panel ctrlCurrentAction;
      private System.Windows.Forms.Panel panel2;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.PictureBox ctrlProfilePic;
      private System.Windows.Forms.Label ctrlEmail;
      private System.Windows.Forms.Label ctrlUsername;
      private System.Windows.Forms.LinkLabel ctrlUploadVideo;
      private System.Windows.Forms.LinkLabel ctrlListLiveEvents;
      private System.Windows.Forms.LinkLabel ctrlCreateLiveEvent;

   }
}
