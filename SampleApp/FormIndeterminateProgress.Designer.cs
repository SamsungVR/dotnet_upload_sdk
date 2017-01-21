namespace SampleApp {
   partial class FormIndeterminateProgress {
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
         this.progress = new System.Windows.Forms.ProgressBar();
         this.msg = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // progress
         // 
         this.progress.Cursor = System.Windows.Forms.Cursors.Default;
         this.progress.Location = new System.Drawing.Point(33, 63);
         this.progress.MarqueeAnimationSpeed = 30;
         this.progress.Name = "progress";
         this.progress.Size = new System.Drawing.Size(309, 18);
         this.progress.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
         this.progress.TabIndex = 0;
         // 
         // msg
         // 
         this.msg.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.msg.Location = new System.Drawing.Point(30, 14);
         this.msg.Name = "msg";
         this.msg.Size = new System.Drawing.Size(312, 33);
         this.msg.TabIndex = 1;
         this.msg.Text = "label1";
         this.msg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

         // 
         // FormIndeterminateProgress
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.msg);
         this.Controls.Add(this.progress);
         this.Name = "FormIndeterminateProgress";
         this.Size = new System.Drawing.Size(362, 97);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.ProgressBar progress;
      private System.Windows.Forms.Label msg;
   }
}
