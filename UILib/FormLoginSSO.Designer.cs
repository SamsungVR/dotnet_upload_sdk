namespace UILib {
   partial class FormLoginSSO {
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
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlBack = new System.Windows.Forms.Button();
         this.ctrlLoginPage = new System.Windows.Forms.Button();
         this.ctrlWebView = new System.Windows.Forms.WebBrowser();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlBack);
         this.groupBox1.Controls.Add(this.ctrlLoginPage);
         this.groupBox1.Location = new System.Drawing.Point(3, 3);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(624, 45);
         this.groupBox1.TabIndex = 3;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Options";
         // 
         // ctrlBack
         // 
         this.ctrlBack.Location = new System.Drawing.Point(129, 16);
         this.ctrlBack.Name = "ctrlBack";
         this.ctrlBack.Size = new System.Drawing.Size(75, 23);
         this.ctrlBack.TabIndex = 4;
         this.ctrlBack.Text = "Go Back";
         this.ctrlBack.UseVisualStyleBackColor = true;
         this.ctrlBack.Click += new System.EventHandler(this.ctrlBack_Click);
         // 
         // ctrlLoginPage
         // 
         this.ctrlLoginPage.Location = new System.Drawing.Point(6, 16);
         this.ctrlLoginPage.Name = "ctrlLoginPage";
         this.ctrlLoginPage.Size = new System.Drawing.Size(117, 23);
         this.ctrlLoginPage.TabIndex = 3;
         this.ctrlLoginPage.Text = "Go to Login page";
         this.ctrlLoginPage.UseVisualStyleBackColor = true;
         this.ctrlLoginPage.Click += new System.EventHandler(this.ctrlLoginPage_Click);
         // 
         // ctrlWebView
         // 
         this.ctrlWebView.AllowWebBrowserDrop = false;
         this.ctrlWebView.IsWebBrowserContextMenuEnabled = false;
         this.ctrlWebView.Location = new System.Drawing.Point(0, 54);
         this.ctrlWebView.Margin = new System.Windows.Forms.Padding(0);
         this.ctrlWebView.MinimumSize = new System.Drawing.Size(20, 20);
         this.ctrlWebView.Name = "ctrlWebView";
         this.ctrlWebView.ScriptErrorsSuppressed = true;
         this.ctrlWebView.Size = new System.Drawing.Size(630, 290);
         this.ctrlWebView.TabIndex = 4;
         this.ctrlWebView.WebBrowserShortcutsEnabled = false;
         this.ctrlWebView.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.ctrlWebView_onNavigating);
         // 
         // FormLoginSSO
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.Controls.Add(this.ctrlWebView);
         this.Controls.Add(this.groupBox1);
         this.Name = "FormLoginSSO";
         this.Size = new System.Drawing.Size(630, 350);
         this.groupBox1.ResumeLayout(false);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button ctrlBack;
      private System.Windows.Forms.Button ctrlLoginPage;
      private System.Windows.Forms.WebBrowser ctrlWebView;
   }
}
