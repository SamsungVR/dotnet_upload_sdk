using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {

   public partial class FormIndeterminateProgress : UserControl {


      public FormIndeterminateProgress(string caption) {
         InitializeComponent();
         msg.Text = caption;
      }

   }
}
