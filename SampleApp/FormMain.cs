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

      class CallbackVRDestroy : VR.Result.Destroy.If {

         private readonly FormMain mFormMain;

         public CallbackVRDestroy(FormMain formMain) {
            mFormMain = formMain;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackVRDestroy));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "destroy failed status: " + status);
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "destroy success");
            App app = App.getInstance();
            app.quit();
         }

      }

      class CallbackUILib : UILib.UILib.Callback {

         private readonly FormMain mFormMain;

         public CallbackUILib(FormMain formMain) {
            mFormMain = formMain;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackVRDestroy));

         public void onLibInitStatus(object closure, bool status) {
         }

         public void onLibDestroyStatus(object closure, bool status) {
            if (status) {
               Log.d(TAG, "destroy success");
               App app = App.getInstance();
               app.quit();
            }
         }

         public void onLoginSuccess(SDKLib.User.If user, object closure) {
         }

         public void onLoginFailure(object closure) {
         }

         public void onLogoutSuccess(User.If user, object closure) {
         }

         public void onLogoutFailure(User.If user, object closure) {
         }

         public void showLoginUI(UserControl loginUI, object closure) {
         }
      }

      private readonly CallbackUILib mCallbackUILib;
      private readonly CallbackVRDestroy mCallbackVRDestroy;

      public FormMain() {
         InitializeComponent();

         mCallbackUILib = new CallbackUILib(this);
         mCallbackVRDestroy = new CallbackVRDestroy(this);
      }

      protected override void OnFormClosing(FormClosingEventArgs e) {
         base.OnFormClosing(e);
         App app = App.getInstance();

         bool defer = false;
         if (App.USE_UILIB) {
            defer = UILib.UILib.destroy();
         } else {
            defer = VR.destroyAsync(mCallbackVRDestroy, app.getHandler(), null);
         }
         Log.flushLogFile();
         if (defer) {
            FormDialog dialog = app.showDialog();
            dialog.setControl(new FormIndeterminateProgress(ResourceStrings.deinitVR));
            e.Cancel = true;
         }
      }

      protected override void OnFormClosed(FormClosedEventArgs e) {
         base.OnFormClosed(e);
         App.getInstance().mUILibCallback.mSubCallbacks.Remove(mCallbackUILib);
      }

      private void OnFormLoad(object sender, EventArgs e) {
         App.getInstance().mUILibCallback.mSubCallbacks.Add(mCallbackUILib);
      }

   }
}
