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
         this.components = new System.ComponentModel.Container();
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormLogin));
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlVREmail = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlVRPassword = new System.Windows.Forms.TextBox();
         this.ctrlShowPassword = new System.Windows.Forms.CheckBox();
         this.ctrlLogin = new System.Windows.Forms.Button();
         this.imageListLogin = new System.Windows.Forms.ImageList(this.components);
         this.label3 = new System.Windows.Forms.Label();
         this.ctrlWebView = new System.Windows.Forms.WebBrowser();
         this.label4 = new System.Windows.Forms.Label();
         this.ctrlGoHome = new System.Windows.Forms.Button();
         this.imageListHome = new System.Windows.Forms.ImageList(this.components);
         this.ctrlGoBack = new System.Windows.Forms.Button();
         this.imageListBack = new System.Windows.Forms.ImageList(this.components);
         this.SuspendLayout();
         // 
         // label1
         // 
         this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.ForeColor = System.Drawing.Color.White;
         this.label1.Location = new System.Drawing.Point(40, 418);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(220, 25);
         this.label1.TabIndex = 0;
         this.label1.Text = "Samsung VR Email ...";
         // 
         // ctrlVREmail
         // 
         this.ctrlVREmail.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlVREmail.BackColor = System.Drawing.Color.DimGray;
         this.ctrlVREmail.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ctrlVREmail.ForeColor = System.Drawing.Color.White;
         this.ctrlVREmail.Location = new System.Drawing.Point(45, 446);
         this.ctrlVREmail.Name = "ctrlVREmail";
         this.ctrlVREmail.Size = new System.Drawing.Size(389, 31);
         this.ctrlVREmail.TabIndex = 1;
         // 
         // label2
         // 
         this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.label2.AutoSize = true;
         this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label2.ForeColor = System.Drawing.Color.White;
         this.label2.Location = new System.Drawing.Point(40, 490);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(261, 25);
         this.label2.TabIndex = 2;
         this.label2.Text = "Samsung VR Password ...";
         // 
         // ctrlVRPassword
         // 
         this.ctrlVRPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlVRPassword.BackColor = System.Drawing.Color.DimGray;
         this.ctrlVRPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ctrlVRPassword.ForeColor = System.Drawing.Color.White;
         this.ctrlVRPassword.Location = new System.Drawing.Point(45, 518);
         this.ctrlVRPassword.Name = "ctrlVRPassword";
         this.ctrlVRPassword.Size = new System.Drawing.Size(389, 31);
         this.ctrlVRPassword.TabIndex = 3;
         this.ctrlVRPassword.UseSystemPasswordChar = true;
         // 
         // ctrlShowPassword
         // 
         this.ctrlShowPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlShowPassword.AutoSize = true;
         this.ctrlShowPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.ctrlShowPassword.ForeColor = System.Drawing.Color.White;
         this.ctrlShowPassword.Location = new System.Drawing.Point(45, 555);
         this.ctrlShowPassword.Name = "ctrlShowPassword";
         this.ctrlShowPassword.Size = new System.Drawing.Size(182, 29);
         this.ctrlShowPassword.TabIndex = 4;
         this.ctrlShowPassword.Text = "Show password";
         this.ctrlShowPassword.UseVisualStyleBackColor = true;
         this.ctrlShowPassword.CheckedChanged += new System.EventHandler(this.ctrlShowPassword_CheckedChanged);
         // 
         // ctrlLogin
         // 
         this.ctrlLogin.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlLogin.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
         this.ctrlLogin.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
         this.ctrlLogin.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
         this.ctrlLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.ctrlLogin.ImageIndex = 0;
         this.ctrlLogin.ImageList = this.imageListLogin;
         this.ctrlLogin.Location = new System.Drawing.Point(221, 609);
         this.ctrlLogin.Name = "ctrlLogin";
         this.ctrlLogin.Size = new System.Drawing.Size(36, 37);
         this.ctrlLogin.TabIndex = 5;
         this.ctrlLogin.UseVisualStyleBackColor = true;
         this.ctrlLogin.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseDown);
         this.ctrlLogin.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseUp);
         // 
         // imageListLogin
         // 
         this.imageListLogin.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListLogin.ImageStream")));
         this.imageListLogin.TransparentColor = System.Drawing.Color.Transparent;
         this.imageListLogin.Images.SetKeyName(0, "ic_check_circle_white_36dp.png");
         this.imageListLogin.Images.SetKeyName(1, "ic_check_circle_black_36dp.png");
         // 
         // label3
         // 
         this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.label3.AutoSize = true;
         this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label3.ForeColor = System.Drawing.Color.White;
         this.label3.Location = new System.Drawing.Point(40, 37);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(179, 25);
         this.label3.TabIndex = 6;
         this.label3.Text = "Single Sign On ...";
         // 
         // ctrlWebView
         // 
         this.ctrlWebView.AllowWebBrowserDrop = false;
         this.ctrlWebView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlWebView.IsWebBrowserContextMenuEnabled = false;
         this.ctrlWebView.Location = new System.Drawing.Point(45, 79);
         this.ctrlWebView.Margin = new System.Windows.Forms.Padding(0);
         this.ctrlWebView.MinimumSize = new System.Drawing.Size(20, 20);
         this.ctrlWebView.Name = "ctrlWebView";
         this.ctrlWebView.ScriptErrorsSuppressed = true;
         this.ctrlWebView.Size = new System.Drawing.Size(389, 262);
         this.ctrlWebView.TabIndex = 7;
         this.ctrlWebView.WebBrowserShortcutsEnabled = false;
         this.ctrlWebView.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.ctrlWebView_onNavigating);
         // 
         // label4
         // 
         this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
         this.label4.AutoSize = true;
         this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label4.ForeColor = System.Drawing.Color.White;
         this.label4.Location = new System.Drawing.Point(222, 368);
         this.label4.Name = "label4";
         this.label4.Size = new System.Drawing.Size(35, 25);
         this.label4.TabIndex = 8;
         this.label4.Text = "Or";
         // 
         // ctrlGoHome
         // 
         this.ctrlGoHome.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlGoHome.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
         this.ctrlGoHome.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
         this.ctrlGoHome.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
         this.ctrlGoHome.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.ctrlGoHome.ImageIndex = 0;
         this.ctrlGoHome.ImageList = this.imageListHome;
         this.ctrlGoHome.Location = new System.Drawing.Point(398, 33);
         this.ctrlGoHome.Name = "ctrlGoHome";
         this.ctrlGoHome.Size = new System.Drawing.Size(36, 37);
         this.ctrlGoHome.TabIndex = 9;
         this.ctrlGoHome.UseVisualStyleBackColor = true;
         this.ctrlGoHome.Click += new System.EventHandler(this.ctrlGoHome_Click);
         this.ctrlGoHome.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseDown);
         this.ctrlGoHome.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseUp);
         // 
         // imageListHome
         // 
         this.imageListHome.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListHome.ImageStream")));
         this.imageListHome.TransparentColor = System.Drawing.Color.Transparent;
         this.imageListHome.Images.SetKeyName(0, "ic_home_white_36dp.png");
         this.imageListHome.Images.SetKeyName(1, "ic_home_black_36dp.png");
         // 
         // ctrlGoBack
         // 
         this.ctrlGoBack.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
         this.ctrlGoBack.FlatAppearance.BorderColor = System.Drawing.Color.DimGray;
         this.ctrlGoBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.DimGray;
         this.ctrlGoBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DimGray;
         this.ctrlGoBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
         this.ctrlGoBack.ImageIndex = 0;
         this.ctrlGoBack.ImageList = this.imageListBack;
         this.ctrlGoBack.Location = new System.Drawing.Point(356, 33);
         this.ctrlGoBack.Name = "ctrlGoBack";
         this.ctrlGoBack.Size = new System.Drawing.Size(36, 37);
         this.ctrlGoBack.TabIndex = 10;
         this.ctrlGoBack.UseVisualStyleBackColor = true;
         this.ctrlGoBack.Click += new System.EventHandler(this.ctrlgoBack_Click);
         this.ctrlGoBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseDown);
         this.ctrlGoBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ctrlButton_MouseUp);
         // 
         // imageListBack
         // 
         this.imageListBack.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBack.ImageStream")));
         this.imageListBack.TransparentColor = System.Drawing.Color.Transparent;
         this.imageListBack.Images.SetKeyName(0, "ic_arrow_back_white_36dp.png");
         this.imageListBack.Images.SetKeyName(1, "ic_arrow_back_black_36dp.png");
         // 
         // FormLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.AutoSize = true;
         this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
         this.BackColor = System.Drawing.Color.DimGray;
         this.Controls.Add(this.ctrlGoBack);
         this.Controls.Add(this.ctrlGoHome);
         this.Controls.Add(this.label4);
         this.Controls.Add(this.ctrlWebView);
         this.Controls.Add(this.label3);
         this.Controls.Add(this.ctrlLogin);
         this.Controls.Add(this.ctrlShowPassword);
         this.Controls.Add(this.ctrlVRPassword);
         this.Controls.Add(this.label2);
         this.Controls.Add(this.ctrlVREmail);
         this.Controls.Add(this.label1);
         this.Name = "FormLogin";
         this.Size = new System.Drawing.Size(478, 664);
         this.Load += new System.EventHandler(this.FormLogin_Load);
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.TextBox ctrlVREmail;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.TextBox ctrlVRPassword;
      private System.Windows.Forms.CheckBox ctrlShowPassword;
      private System.Windows.Forms.Button ctrlLogin;
      private System.Windows.Forms.ImageList imageListLogin;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.WebBrowser ctrlWebView;
      private System.Windows.Forms.Label label4;
      private System.Windows.Forms.Button ctrlGoHome;
      private System.Windows.Forms.Button ctrlGoBack;
      private System.Windows.Forms.ImageList imageListHome;
      private System.Windows.Forms.ImageList imageListBack;



   }
}
