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
         this.ctrlCancel = new System.Windows.Forms.Button();
         this.ctrlApply = new System.Windows.Forms.Button();
         this.ctrlAPIKey = new System.Windows.Forms.TextBox();
         this.label2 = new System.Windows.Forms.Label();
         this.ctrlUrl = new System.Windows.Forms.TextBox();
         this.label1 = new System.Windows.Forms.Label();
         this.ctrlStatus = new System.Windows.Forms.Label();
         this.ctrlBack = new System.Windows.Forms.Button();
         this.ctrlConfigList = new System.Windows.Forms.FlowLayoutPanel();
         this.groupBox1 = new System.Windows.Forms.GroupBox();
         this.ctrlCfgSave = new System.Windows.Forms.Button();
         this.ctrlCfgLoad = new System.Windows.Forms.Button();
         this.ctrlCfgSelect = new System.Windows.Forms.Button();
         this.ctrlCfgFile = new System.Windows.Forms.TextBox();
         this.ctrlFileChooser = new System.Windows.Forms.SaveFileDialog();
         this.ctrlEditGroup.SuspendLayout();
         this.groupBox1.SuspendLayout();
         this.SuspendLayout();
         // 
         // ctrlAdd
         // 
         this.ctrlAdd.Location = new System.Drawing.Point(16, 150);
         this.ctrlAdd.Name = "ctrlAdd";
         this.ctrlAdd.Size = new System.Drawing.Size(75, 23);
         this.ctrlAdd.TabIndex = 1;
         this.ctrlAdd.Text = "Add";
         this.ctrlAdd.UseVisualStyleBackColor = true;
         this.ctrlAdd.Click += new System.EventHandler(this.ctrlAdd_Click);
         // 
         // ctrlEditGroup
         // 
         this.ctrlEditGroup.Controls.Add(this.ctrlCancel);
         this.ctrlEditGroup.Controls.Add(this.ctrlApply);
         this.ctrlEditGroup.Controls.Add(this.ctrlAPIKey);
         this.ctrlEditGroup.Controls.Add(this.label2);
         this.ctrlEditGroup.Controls.Add(this.ctrlUrl);
         this.ctrlEditGroup.Controls.Add(this.label1);
         this.ctrlEditGroup.Enabled = false;
         this.ctrlEditGroup.Location = new System.Drawing.Point(16, 169);
         this.ctrlEditGroup.Name = "ctrlEditGroup";
         this.ctrlEditGroup.Size = new System.Drawing.Size(356, 133);
         this.ctrlEditGroup.TabIndex = 2;
         this.ctrlEditGroup.TabStop = false;
         // 
         // ctrlCancel
         // 
         this.ctrlCancel.Location = new System.Drawing.Point(274, 97);
         this.ctrlCancel.Name = "ctrlCancel";
         this.ctrlCancel.Size = new System.Drawing.Size(75, 23);
         this.ctrlCancel.TabIndex = 5;
         this.ctrlCancel.Text = "Cancel";
         this.ctrlCancel.UseVisualStyleBackColor = true;
         this.ctrlCancel.Click += new System.EventHandler(this.ctrlCancel_Click);
         // 
         // ctrlApply
         // 
         this.ctrlApply.Location = new System.Drawing.Point(194, 97);
         this.ctrlApply.Name = "ctrlApply";
         this.ctrlApply.Size = new System.Drawing.Size(75, 23);
         this.ctrlApply.TabIndex = 4;
         this.ctrlApply.Text = "Apply";
         this.ctrlApply.UseVisualStyleBackColor = true;
         this.ctrlApply.Click += new System.EventHandler(this.ctrlApply_Click);
         // 
         // ctrlAPIKey
         // 
         this.ctrlAPIKey.Location = new System.Drawing.Point(10, 71);
         this.ctrlAPIKey.Name = "ctrlAPIKey";
         this.ctrlAPIKey.Size = new System.Drawing.Size(340, 20);
         this.ctrlAPIKey.TabIndex = 3;
         // 
         // label2
         // 
         this.label2.AutoSize = true;
         this.label2.Location = new System.Drawing.Point(7, 55);
         this.label2.Name = "label2";
         this.label2.Size = new System.Drawing.Size(45, 13);
         this.label2.TabIndex = 2;
         this.label2.Text = "API Key";
         // 
         // ctrlUrl
         // 
         this.ctrlUrl.Location = new System.Drawing.Point(7, 32);
         this.ctrlUrl.Name = "ctrlUrl";
         this.ctrlUrl.Size = new System.Drawing.Size(343, 20);
         this.ctrlUrl.TabIndex = 1;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Location = new System.Drawing.Point(7, 16);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(65, 13);
         this.label1.TabIndex = 0;
         this.label1.Text = "Endpoint Url";
         // 
         // ctrlStatus
         // 
         this.ctrlStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
         this.ctrlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlStatus.ForeColor = System.Drawing.Color.Red;
         this.ctrlStatus.Location = new System.Drawing.Point(16, 584);
         this.ctrlStatus.Name = "ctrlStatus";
         this.ctrlStatus.Size = new System.Drawing.Size(356, 46);
         this.ctrlStatus.TabIndex = 6;
         this.ctrlStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
         this.ctrlConfigList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
         this.ctrlConfigList.Location = new System.Drawing.Point(16, 316);
         this.ctrlConfigList.Margin = new System.Windows.Forms.Padding(0);
         this.ctrlConfigList.Name = "ctrlConfigList";
         this.ctrlConfigList.Size = new System.Drawing.Size(356, 255);
         this.ctrlConfigList.TabIndex = 4;
         // 
         // groupBox1
         // 
         this.groupBox1.Controls.Add(this.ctrlCfgSave);
         this.groupBox1.Controls.Add(this.ctrlCfgLoad);
         this.groupBox1.Controls.Add(this.ctrlCfgSelect);
         this.groupBox1.Controls.Add(this.ctrlCfgFile);
         this.groupBox1.Location = new System.Drawing.Point(16, 55);
         this.groupBox1.Name = "groupBox1";
         this.groupBox1.Size = new System.Drawing.Size(350, 79);
         this.groupBox1.TabIndex = 5;
         this.groupBox1.TabStop = false;
         this.groupBox1.Text = "Config file";
         // 
         // ctrlCfgSave
         // 
         this.ctrlCfgSave.Location = new System.Drawing.Point(275, 46);
         this.ctrlCfgSave.Name = "ctrlCfgSave";
         this.ctrlCfgSave.Size = new System.Drawing.Size(69, 23);
         this.ctrlCfgSave.TabIndex = 3;
         this.ctrlCfgSave.Text = "Save";
         this.ctrlCfgSave.UseVisualStyleBackColor = true;
         this.ctrlCfgSave.Click += new System.EventHandler(this.ctrlCfgSave_Click);
         // 
         // ctrlCfgLoad
         // 
         this.ctrlCfgLoad.Location = new System.Drawing.Point(209, 45);
         this.ctrlCfgLoad.Name = "ctrlCfgLoad";
         this.ctrlCfgLoad.Size = new System.Drawing.Size(60, 23);
         this.ctrlCfgLoad.TabIndex = 2;
         this.ctrlCfgLoad.Text = "Load";
         this.ctrlCfgLoad.UseVisualStyleBackColor = true;
         this.ctrlCfgLoad.Click += new System.EventHandler(this.ctrlCfgLoad_Click);
         // 
         // ctrlCfgSelect
         // 
         this.ctrlCfgSelect.Location = new System.Drawing.Point(136, 45);
         this.ctrlCfgSelect.Name = "ctrlCfgSelect";
         this.ctrlCfgSelect.Size = new System.Drawing.Size(67, 23);
         this.ctrlCfgSelect.TabIndex = 1;
         this.ctrlCfgSelect.Text = "Select";
         this.ctrlCfgSelect.UseVisualStyleBackColor = true;
         this.ctrlCfgSelect.Click += new System.EventHandler(this.ctrlCfgSelect_Click);
         // 
         // ctrlCfgFile
         // 
         this.ctrlCfgFile.Location = new System.Drawing.Point(10, 19);
         this.ctrlCfgFile.Name = "ctrlCfgFile";
         this.ctrlCfgFile.Size = new System.Drawing.Size(334, 20);
         this.ctrlCfgFile.TabIndex = 0;
         // 
         // ctrlFileChooser
         // 
         this.ctrlFileChooser.CreatePrompt = true;
         this.ctrlFileChooser.FileName = "vrsdkcfg.cfg";
         this.ctrlFileChooser.OverwritePrompt = false;
         // 
         // FormEndPointConfig
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.Window;
         this.Controls.Add(this.ctrlStatus);
         this.Controls.Add(this.groupBox1);
         this.Controls.Add(this.ctrlAdd);
         this.Controls.Add(this.ctrlConfigList);
         this.Controls.Add(this.ctrlBack);
         this.Controls.Add(this.ctrlEditGroup);
         this.Name = "FormEndPointConfig";
         this.Size = new System.Drawing.Size(391, 642);
         this.Load += new System.EventHandler(this.FormEndPointConfig_Load);
         this.Leave += new System.EventHandler(this.FormEndPointConfig_Leave);
         this.ctrlEditGroup.ResumeLayout(false);
         this.ctrlEditGroup.PerformLayout();
         this.groupBox1.ResumeLayout(false);
         this.groupBox1.PerformLayout();
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
      private System.Windows.Forms.Label ctrlStatus;
      private System.Windows.Forms.FlowLayoutPanel ctrlConfigList;
      private System.Windows.Forms.GroupBox groupBox1;
      private System.Windows.Forms.Button ctrlCfgSave;
      private System.Windows.Forms.Button ctrlCfgLoad;
      private System.Windows.Forms.Button ctrlCfgSelect;
      private System.Windows.Forms.TextBox ctrlCfgFile;
      private System.Windows.Forms.SaveFileDialog ctrlFileChooser;
   }
}
