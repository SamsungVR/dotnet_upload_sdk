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
         this.SuspendLayout();
         // 
         // ctrlEndPoint
         // 
         this.ctrlEndPoint.BackColor = System.Drawing.SystemColors.Window;
         this.ctrlEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlEndPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.ctrlEndPoint.Location = new System.Drawing.Point(22, 14);
         this.ctrlEndPoint.Name = "ctrlEndPoint";
         this.ctrlEndPoint.Size = new System.Drawing.Size(478, 26);
         this.ctrlEndPoint.TabIndex = 0;
         this.ctrlEndPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlEndPoint.Click += new System.EventHandler(this.ctrlEndPoint_Click);
         // 
         // ctrlStatusMsg
         // 
         this.ctrlStatusMsg.Location = new System.Drawing.Point(22, 731);
         this.ctrlStatusMsg.Name = "ctrlStatusMsg";
         this.ctrlStatusMsg.Size = new System.Drawing.Size(478, 28);
         this.ctrlStatusMsg.TabIndex = 6;
         this.ctrlStatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlStatusMsg.Click += new System.EventHandler(this.ctrlStatusMsg_Click);
         // 
         // ctrlLoginPanel
         // 
         this.ctrlLoginPanel.Location = new System.Drawing.Point(22, 53);
         this.ctrlLoginPanel.Name = "ctrlLoginPanel";
         this.ctrlLoginPanel.Size = new System.Drawing.Size(478, 664);
         this.ctrlLoginPanel.TabIndex = 7;
         // 
         // FormUILibLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlLoginPanel);
         this.Controls.Add(this.ctrlStatusMsg);
         this.Controls.Add(this.ctrlEndPoint);
         this.Name = "FormUILibLogin";
         this.Size = new System.Drawing.Size(523, 778);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label ctrlEndPoint;
      private System.Windows.Forms.Label ctrlStatusMsg;
      private System.Windows.Forms.Panel ctrlLoginPanel;
   }
}
