﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {
   public partial class FormBase : UserControl {
      public FormBase() {
         InitializeComponent();
      }


      virtual public void onLoad() {
      }

      virtual public void onUnload() {
      }
   }
}
