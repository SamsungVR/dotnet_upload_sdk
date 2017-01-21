using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {
   public partial class FormDialog : FormSnapToContent {

      public FormDialog() {
         InitializeComponent();
      }

      protected override void OnFormClosed(FormClosedEventArgs e) {
         base.OnFormClosed(e);
         App.getInstance().onFormDialogClosed();
      }

   }
}
