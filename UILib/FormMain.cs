using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace UILib {

   internal partial class FormMain : Form {

      private UILib.Impl mUILib;

      public FormMain(UILib.Impl impl) {
         InitializeComponent();
         mUILib = impl;
      }

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

      protected override void OnFormClosed(FormClosedEventArgs e) {
         base.OnFormClosed(e);
         mUILib.onFormClosed(e);
      }

   }
}
