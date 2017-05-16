using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {

   public class FormSub : UserControl {

      public virtual void onAdded() {
      }

      public virtual void onRemoved() {
      }

      private void InitializeComponent() {
         this.SuspendLayout();
         // 
         // FormSub
         // 
         this.Name = "FormSub";
         this.Load += new System.EventHandler(this.FormSub_Load);
         this.ResumeLayout(false);

      }

      private void FormSub_Load(object sender, EventArgs e) {

      }
   }
}
