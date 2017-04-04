namespace SampleApp {
   partial class FormUILibLogin {
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
         this.ctrlEndPoint = new System.Windows.Forms.Label();
         this.ctrlStatusMsg = new System.Windows.Forms.Label();
         this.ctrlLoginPanel = new System.Windows.Forms.Panel();
         this.ctrlWaitPanel = new System.Windows.Forms.Panel();
         this.progressBar1 = new System.Windows.Forms.ProgressBar();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlWaitPanel.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlEndPoint
         // 
         this.ctrlEndPoint.BackColor = System.Drawing.SystemColors.Window;
         this.ctrlEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlEndPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.ctrlEndPoint.Location = new System.Drawing.Point(13, 14);
         this.ctrlEndPoint.Name = "ctrlEndPoint";
         this.ctrlEndPoint.Size = new System.Drawing.Size(400, 26);
         this.ctrlEndPoint.TabIndex = 0;
         this.ctrlEndPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlEndPoint.Click += new System.EventHandler(this.ctrlEndPoint_Click);
         // 
         // ctrlStatusMsg
         // 
         this.ctrlStatusMsg.Location = new System.Drawing.Point(13, 666);
         this.ctrlStatusMsg.Name = "ctrlStatusMsg";
         this.ctrlStatusMsg.Size = new System.Drawing.Size(400, 28);
         this.ctrlStatusMsg.TabIndex = 6;
         this.ctrlStatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlStatusMsg.Click += new System.EventHandler(this.ctrlStatusMsg_Click);
         // 
         // ctrlLoginPanel
         // 
         this.ctrlLoginPanel.Location = new System.Drawing.Point(13, 53);
         this.ctrlLoginPanel.Name = "ctrlLoginPanel";
         this.ctrlLoginPanel.Size = new System.Drawing.Size(400, 600);
         this.ctrlLoginPanel.TabIndex = 7;
         // 
         // ctrlWaitPanel
         // 
         this.ctrlWaitPanel.Controls.Add(this.progressBar1);
         this.ctrlWaitPanel.Controls.Add(this.label1);
         this.ctrlWaitPanel.Location = new System.Drawing.Point(15, 286);
         this.ctrlWaitPanel.Name = "ctrlWaitPanel";
         this.ctrlWaitPanel.Size = new System.Drawing.Size(400, 136);
         this.ctrlWaitPanel.TabIndex = 8;
         // 
         // progressBar1
         // 
         this.progressBar1.Location = new System.Drawing.Point(107, 82);
         this.progressBar1.Name = "progressBar1";
         this.progressBar1.Size = new System.Drawing.Size(186, 23);
         this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
         this.progressBar1.TabIndex = 1;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.BackColor = System.Drawing.Color.White;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(104, 29);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(193, 37);
         this.label1.TabIndex = 0;
         this.label1.Text = "Initializing ...";
         // 
         // FormUILibLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlWaitPanel);
         this.Controls.Add(this.ctrlStatusMsg);
         this.Controls.Add(this.ctrlEndPoint);
         this.Controls.Add(this.ctrlLoginPanel);
         this.Name = "FormUILibLogin";
         this.Size = new System.Drawing.Size(430, 709);
         this.Load += new System.EventHandler(this.FormUILibLogin_Load);
         this.ctrlWaitPanel.ResumeLayout(false);
         this.ctrlWaitPanel.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label ctrlEndPoint;
      private System.Windows.Forms.Label ctrlStatusMsg;
      private System.Windows.Forms.Panel ctrlLoginPanel;
      private System.Windows.Forms.Panel ctrlWaitPanel;
      private System.Windows.Forms.ProgressBar progressBar1;
      private System.Windows.Forms.Label label1;
   }
}
