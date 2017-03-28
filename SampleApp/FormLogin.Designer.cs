namespace SampleApp {
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
         this.ctrlEndPoint = new System.Windows.Forms.Label();
         this.ctrlStatusMsg = new System.Windows.Forms.Label();
         this.ctrlCredsGroup = new System.Windows.Forms.GroupBox();
         this.button1 = new System.Windows.Forms.Button();
         this.ctrlLogin = new System.Windows.Forms.Button();
         this.ctrlPassword = new System.Windows.Forms.TextBox();
         this.label3 = new System.Windows.Forms.Label();
         this.ctrlUsername = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlCredsGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlEndPoint
         // 
         this.ctrlEndPoint.BackColor = System.Drawing.SystemColors.Window;
         this.ctrlEndPoint.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlEndPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
         this.ctrlEndPoint.Location = new System.Drawing.Point(22, 14);
         this.ctrlEndPoint.Name = "ctrlEndPoint";
         this.ctrlEndPoint.Size = new System.Drawing.Size(263, 26);
         this.ctrlEndPoint.TabIndex = 0;
         this.ctrlEndPoint.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlEndPoint.Click += new System.EventHandler(this.ctrlEndPoint_Click);
         // 
         // ctrlStatusMsg
         // 
         this.ctrlStatusMsg.Location = new System.Drawing.Point(22, 264);
         this.ctrlStatusMsg.Name = "ctrlStatusMsg";
         this.ctrlStatusMsg.Size = new System.Drawing.Size(263, 32);
         this.ctrlStatusMsg.TabIndex = 6;
         this.ctrlStatusMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
         this.ctrlStatusMsg.Click += new System.EventHandler(this.ctrlStatusMsg_Click);
         // 
         // ctrlCredsGroup
         // 
         this.ctrlCredsGroup.Controls.Add(this.button1);
         this.ctrlCredsGroup.Controls.Add(this.ctrlLogin);
         this.ctrlCredsGroup.Controls.Add(this.ctrlPassword);
         this.ctrlCredsGroup.Controls.Add(this.label3);
         this.ctrlCredsGroup.Controls.Add(this.ctrlUsername);
         this.ctrlCredsGroup.Controls.Add(this.label2);
         this.ctrlCredsGroup.Location = new System.Drawing.Point(22, 63);
         this.ctrlCredsGroup.Name = "ctrlCredsGroup";
         this.ctrlCredsGroup.Size = new System.Drawing.Size(263, 182);
         this.ctrlCredsGroup.TabIndex = 7;
         this.ctrlCredsGroup.TabStop = false;
         this.ctrlCredsGroup.Text = "Credentials";
         this.ctrlCredsGroup.Enter += new System.EventHandler(this.ctrlCredsGroup_Enter);
         // 
         // button1
         // 
         this.button1.Location = new System.Drawing.Point(100, 149);
         this.button1.Name = "button1";
         this.button1.Size = new System.Drawing.Size(75, 23);
         this.button1.TabIndex = 11;
         this.button1.Text = "Test";
         this.button1.UseVisualStyleBackColor = true;
         this.button1.Click += new System.EventHandler(this.button1_Click);
         // 
         // ctrlLogin
         // 
         this.ctrlLogin.Location = new System.Drawing.Point(182, 149);
         this.ctrlLogin.Name = "ctrlLogin";
         this.ctrlLogin.Size = new System.Drawing.Size(75, 23);
         this.ctrlLogin.TabIndex = 10;
         this.ctrlLogin.Text = "Login";
         this.ctrlLogin.UseVisualStyleBackColor = true;
         this.ctrlLogin.Click += new System.EventHandler(this.ctrlLogin_Click);
         // 
         // ctrlPassword
         // 
         this.ctrlPassword.Location = new System.Drawing.Point(7, 93);
         this.ctrlPassword.Name = "ctrlPassword";
         this.ctrlPassword.Size = new System.Drawing.Size(248, 20);
         this.ctrlPassword.TabIndex = 9;
         // 
         // label3
         // 
         this.label3.AutoSize = true;
         this.label3.Location = new System.Drawing.Point(8, 77);
         this.label3.Name = "label3";
         this.label3.Size = new System.Drawing.Size(53, 13);
         this.label3.TabIndex = 8;
         this.label3.Text = "Password";
         // 
         // ctrlUsername
         // 
         this.ctrlUsername.Location = new System.Drawing.Point(7, 42);
         this.ctrlUsername.Name = "ctrlUsername";
         this.ctrlUsername.Size = new System.Drawing.Size(248, 20);
         this.ctrlUsername.TabIndex = 7;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(6, 26);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(89, 13);
         this.label2.TabIndex = 6;
         this.label2.Text = "Username (Email)";
         // 
         // FormLogin
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlCredsGroup);
         this.Controls.Add(this.ctrlStatusMsg);
         this.Controls.Add(this.ctrlEndPoint);
         this.Name = "FormLogin";
         this.Size = new System.Drawing.Size(307, 310);
         this.Load += new System.EventHandler(this.FormLogin_Load);
         this.ctrlCredsGroup.ResumeLayout(false);
         this.ctrlCredsGroup.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label ctrlEndPoint;
      private System.Windows.Forms.Label ctrlStatusMsg;
      private System.Windows.Forms.GroupBox ctrlCredsGroup;
      private System.Windows.Forms.Button ctrlLogin;
      private System.Windows.Forms.TextBox ctrlPassword;
      private System.Windows.Forms.Label label3;
      private System.Windows.Forms.TextBox ctrlUsername;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button button1;
   }
}
