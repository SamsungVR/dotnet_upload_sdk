namespace SampleApp {
   partial class FormEndPointConfig {
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
         this.ctrlAdd = new System.Windows.Forms.Button();
         this.ctrlEditGroup = new System.Windows.Forms.GroupBox();
         this.ctrlApplyStatus = new System.Windows.Forms.Label();
         this.ctrlCancel = new System.Windows.Forms.Button();
         this.ctrlApply = new System.Windows.Forms.Button();
         this.ctrlAPIKey = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlUrl = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlBack = new System.Windows.Forms.Button();
         this.ctrlConfigList = new System.Windows.Forms.FlowLayoutPanel();
         this.ctrlEditGroup.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlAdd
         // 
         this.ctrlAdd.Location = new System.Drawing.Point(151, 12);
         this.ctrlAdd.Name = "ctrlAdd";
         this.ctrlAdd.Size = new System.Drawing.Size(221, 23);
         this.ctrlAdd.TabIndex = 1;
         this.ctrlAdd.Text = "Add";
         this.ctrlAdd.UseVisualStyleBackColor = true;
         this.ctrlAdd.Click += new System.EventHandler(this.ctrlAdd_Click);
         // 
         // ctrlEditGroup
         // 
         this.ctrlEditGroup.Controls.Add(this.ctrlApplyStatus);
         this.ctrlEditGroup.Controls.Add(this.ctrlCancel);
         this.ctrlEditGroup.Controls.Add(this.ctrlApply);
         this.ctrlEditGroup.Controls.Add(this.ctrlAPIKey);
         this.ctrlEditGroup.Controls.Add(this.label2);
         this.ctrlEditGroup.Controls.Add(this.ctrlUrl);
         this.ctrlEditGroup.Controls.Add(this.label1);
         this.ctrlEditGroup.Enabled = false;
         this.ctrlEditGroup.Location = new System.Drawing.Point(16, 41);
         this.ctrlEditGroup.Name = "ctrlEditGroup";
         this.ctrlEditGroup.Size = new System.Drawing.Size(356, 177);
         this.ctrlEditGroup.TabIndex = 2;
         this.ctrlEditGroup.TabStop = false;
         // 
         // ctrlApplyStatus
         // 
         this.ctrlApplyStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ctrlApplyStatus.Location = new System.Drawing.Point(7, 116);
         this.ctrlApplyStatus.Name = "ctrlApplyStatus";
         this.ctrlApplyStatus.Size = new System.Drawing.Size(343, 23);
         this.ctrlApplyStatus.TabIndex = 6;
         this.ctrlApplyStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlCancel
         // 
         this.ctrlCancel.Location = new System.Drawing.Point(275, 148);
         this.ctrlCancel.Name = "ctrlCancel";
         this.ctrlCancel.Size = new System.Drawing.Size(75, 23);
         this.ctrlCancel.TabIndex = 5;
         this.ctrlCancel.Text = "Cancel";
         this.ctrlCancel.UseVisualStyleBackColor = true;
         this.ctrlCancel.Click += new System.EventHandler(this.ctrlCancel_Click);
         // 
         // ctrlApply
         // 
         this.ctrlApply.Location = new System.Drawing.Point(194, 148);
         this.ctrlApply.Name = "ctrlApply";
         this.ctrlApply.Size = new System.Drawing.Size(75, 23);
         this.ctrlApply.TabIndex = 4;
         this.ctrlApply.Text = "Apply";
         this.ctrlApply.UseVisualStyleBackColor = true;
         this.ctrlApply.Click += new System.EventHandler(this.ctrlApply_Click);
         // 
         // ctrlAPIKey
         // 
         this.ctrlAPIKey.Location = new System.Drawing.Point(10, 81);
         this.ctrlAPIKey.Name = "ctrlAPIKey";
         this.ctrlAPIKey.Size = new System.Drawing.Size(340, 20);
         this.ctrlAPIKey.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(10, 64);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(45, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "API Key";
         // 
         // ctrlUrl
         // 
         this.ctrlUrl.Location = new System.Drawing.Point(7, 37);
         this.ctrlUrl.Name = "ctrlUrl";
         this.ctrlUrl.Size = new System.Drawing.Size(343, 20);
         this.ctrlUrl.TabIndex = 1;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 20);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(65, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Endpoint Url";
         // 
         // ctrlBack
         // 
         this.ctrlBack.Location = new System.Drawing.Point(16, 12);
         this.ctrlBack.Name = "ctrlBack";
         this.ctrlBack.Size = new System.Drawing.Size(129, 23);
         this.ctrlBack.TabIndex = 3;
         this.ctrlBack.Text = "<- Back to Login";
         this.ctrlBack.UseVisualStyleBackColor = true;
         this.ctrlBack.Click += new System.EventHandler(this.ctrlBack_Click);
         // 
         // ctrlConfigList
         // 
         this.ctrlConfigList.AutoScroll = true;
         this.ctrlConfigList.Location = new System.Drawing.Point(16, 235);
         this.ctrlConfigList.Margin = new System.Windows.Forms.Padding(0);
         this.ctrlConfigList.Name = "ctrlConfigList";
         this.ctrlConfigList.Size = new System.Drawing.Size(356, 249);
         this.ctrlConfigList.TabIndex = 4;
         // 
         // FormEndPointConfig
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlConfigList);
         this.Controls.Add(this.ctrlBack);
         this.Controls.Add(this.ctrlEditGroup);
         this.Controls.Add(this.ctrlAdd);
         this.Name = "FormEndPointConfig";
         this.Size = new System.Drawing.Size(389, 500);
         this.Load += new System.EventHandler(this.FormEndPointConfig_Load);
         this.Leave += new System.EventHandler(this.FormEndPointConfig_Leave);
         this.ctrlEditGroup.ResumeLayout(false);
         this.ctrlEditGroup.PerformLayout();
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Button ctrlAdd;
      private System.Windows.Forms.GroupBox ctrlEditGroup;
      private System.Windows.Forms.TextBox ctrlUrl;
      private System.Windows.Forms.Label label1;
      private System.Windows.Forms.Label label2;
      private System.Windows.Forms.Button ctrlCancel;
      private System.Windows.Forms.Button ctrlApply;
      private System.Windows.Forms.TextBox ctrlAPIKey;
      private System.Windows.Forms.Button ctrlBack;
      private System.Windows.Forms.Label ctrlApplyStatus;
      private System.Windows.Forms.FlowLayoutPanel ctrlConfigList;
   }
}
