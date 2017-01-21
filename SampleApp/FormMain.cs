using SDKLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SampleApp {

   public partial class FormMain : FormSnapToContent {

      class CallbackDestroy : VR.Result.Destroy.If {

         private readonly FormMain mFormMain;

         public CallbackDestroy(FormMain formMain) {
            mFormMain = formMain;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackDestroy));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "destroy failed status: " + status);
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "destroy success");
            App app = App.getInstance();
            app.quit();
         }

      }

      private readonly CallbackDestroy mCallbackDestroy;

      public FormMain() {
         InitializeComponent();
         mCallbackDestroy = new CallbackDestroy(this);
      }

      protected override void OnFormClosing(FormClosingEventArgs e) {
         base.OnFormClosing(e);

         App app = App.getInstance();
         if (VR.destroyAsync(mCallbackDestroy, app.getHandler(), null)) {
            FormDialog dialog = app.showDialog();
            dialog.setControl(new FormIndeterminateProgress(ResourceStrings.deinitVR));
         } else {
            e.Cancel = true;
         }
      }

   }
}
