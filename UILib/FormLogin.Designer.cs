namespace UILib {
   partial class FormLogin {
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
         this.ctrlLoginViaLabel = new System.Windows.Forms.Label();
         this.ctrlProviders = new System.Windows.Forms.ComboBox();
         this.ctrlProviderZone = new System.Windows.Forms.Panel();
         this.SuspendLayout();
         // 
         // ctrlLoginViaLabel
         // 
         this.ctrlLoginViaLabel.AutoSize = true;
         this.ctrlLoginViaLabel.Location = new System.Drawing.Point(290, 9);
         this.ctrlLoginViaLabel.Name = "ctrlLoginViaLabel";
         this.ctrlLoginViaLabel.Size = new System.Drawing.Size(50, 13);
         this.ctrlLoginViaLabel.TabIndex = 1;
         this.ctrlLoginViaLabel.Text = "Login via";
         // 
         // ctrlProviders
         // 
         this.ctrlProviders.Anchor = System.Windows.Forms.AnchorStyles.None;
         this.ctrlProviders.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
         this.ctrlProviders.FormattingEnabled = true;
         this.ctrlProviders.Location = new System.Drawing.Point(182, 25);
         this.ctrlProviders.Name = "ctrlProviders";
         this.ctrlProviders.Size = new System.Drawing.Size(266, 21);
         this.ctrlProviders.TabIndex = 2;
         this.ctrlProviders.SelectedIndexChanged += new System.EventHandler(this.ctrlProviders_SelectedIndexChanged);
         // 
         // ctrlProviderZone
         // 
         this.ctrlProviderZone.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlProviderZone.Location = new System.Drawing.Point(0, 58);
         this.ctrlProviderZone.Margin = new System.Windows.Forms.Padding(0);
         this.ctrlProviderZone.Name = "ctrlProviderZone";
         this.ctrlProviderZone.Size = new System.Drawing.Size(630, 350);
         this.ctrlProviderZone.TabIndex = 3;
         // 
         // FormLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.Color.White;
         this.Controls.Add(this.ctrlProviderZone);
         this.Controls.Add(this.ctrlProviders);
         this.Controls.Add(this.ctrlLoginViaLabel);
         this.Name = "FormLogin";
         this.Size = new System.Drawing.Size(630, 470);
         this.Load += new System.EventHandler(this.FormLogin_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label ctrlLoginViaLabel;
      private System.Windows.Forms.ComboBox ctrlProviders;
      private System.Windows.Forms.Panel ctrlProviderZone;
   }
}
