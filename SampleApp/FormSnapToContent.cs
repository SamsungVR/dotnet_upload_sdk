using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {

   public partial class FormSnapToContent : Form {

      private Control mControl;

      public void setControl(Control control) {
         if (mControl == control) {
            return;
         }
         if (null != mControl) {
            Controls.Remove(mControl);
         }
         mControl = control;
         if (null != mControl) {
            Controls.Add(mControl);
            snapToControl();
         }
      }


      private void snapToControl() {
         if (null != mControl) {
            SetClientSizeCore(mControl.Width, mControl.Height);
         }
      }

      private void FormSnapToContent_Load(object sender, EventArgs e) {

      }


   }
}
