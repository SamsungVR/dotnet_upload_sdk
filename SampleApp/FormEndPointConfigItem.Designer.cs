namespace SampleApp {
   partial class FormEndPointConfigItem {
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
         this.ctrlUrl = new System.Windows.Forms.Label();
         this.ctrlApiKey = new System.Windows.Forms.Label();
         this.ctrlEdit = new System.Windows.Forms.Button();
         this.ctrlDelete = new System.Windows.Forms.Button();
         this.ctrlSelect = new System.Windows.Forms.Button();
         this.SuspendLayout();
         // 
         // ctrlUrl
         // 
         this.ctrlUrl.Location = new System.Drawing.Point(9, 8);
         this.ctrlUrl.Name = "ctrlUrl";
         this.ctrlUrl.Size = new System.Drawing.Size(290, 17);
         this.ctrlUrl.TabIndex = 0;
         this.ctrlUrl.Text = "asdf";
         this.ctrlUrl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlApiKey
         // 
         this.ctrlApiKey.Location = new System.Drawing.Point(9, 28);
         this.ctrlApiKey.Name = "ctrlApiKey";
         this.ctrlApiKey.Size = new System.Drawing.Size(290, 17);
         this.ctrlApiKey.TabIndex = 1;
         this.ctrlApiKey.Text = "asdf2";
         this.ctrlApiKey.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
         // 
         // ctrlEdit
         // 
         this.ctrlEdit.Location = new System.Drawing.Point(157, 48);
         this.ctrlEdit.Name = "ctrlEdit";
         this.ctrlEdit.Size = new System.Drawing.Size(75, 23);
         this.ctrlEdit.TabIndex = 2;
         this.ctrlEdit.Text = "Edit";
         this.ctrlEdit.UseVisualStyleBackColor = true;
         this.ctrlEdit.Click += new System.EventHandler(this.ctrlEdit_Click);
         // 
         // ctrlDelete
         // 
         this.ctrlDelete.Location = new System.Drawing.Point(238, 48);
         this.ctrlDelete.Name = "ctrlDelete";
         this.ctrlDelete.Size = new System.Drawing.Size(75, 23);
         this.ctrlDelete.TabIndex = 3;
         this.ctrlDelete.Text = "Delete";
         this.ctrlDelete.UseVisualStyleBackColor = true;
         this.ctrlDelete.Click += new System.EventHandler(this.ctrlDelete_Click);
         // 
         // ctrlSelect
         // 
         this.ctrlSelect.Location = new System.Drawing.Point(76, 48);
         this.ctrlSelect.Name = "ctrlSelect";
         this.ctrlSelect.Size = new System.Drawing.Size(75, 23);
         this.ctrlSelect.TabIndex = 4;
         this.ctrlSelect.Text = "Select";
         this.ctrlSelect.UseVisualStyleBackColor = true;
         this.ctrlSelect.Click += new System.EventHandler(this.ctrlSelect_Click);
         // 
         // FormEndPointConfigItem
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.BackColor = System.Drawing.SystemColors.ControlLight;
         this.Controls.Add(this.ctrlSelect);
         this.Controls.Add(this.ctrlDelete);
         this.Controls.Add(this.ctrlEdit);
         this.Controls.Add(this.ctrlApiKey);
         this.Controls.Add(this.ctrlUrl);
         this.Name = "FormEndPointConfigItem";
         this.Size = new System.Drawing.Size(320, 79);
         this.Load += new System.EventHandler(this.FormEndPointConfigItem_Load);
         this.ResumeLayout(false);

      }

      #endregion

      private System.Windows.Forms.Label ctrlUrl;
      private System.Windows.Forms.Label ctrlApiKey;
      private System.Windows.Forms.Button ctrlEdit;
      private System.Windows.Forms.Button ctrlDelete;
      private System.Windows.Forms.Button ctrlSelect;
   }
}
