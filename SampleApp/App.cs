using SDKLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace SampleApp {

   class App {

      private static App sApp;

      private App() {
      }

      private FormMain mFormMain;

      public void main() {
         Application.EnableVisualStyles();
         WindowsFormsSynchronizationContext handler = new WindowsFormsSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(handler);
         mFormMain = new FormMain();
         mFormMain.Show();
         mFormMain.setControl(new FormLogin());
         Application.Run();

         mFormDialog = null;
         mFormMain = null;
      }

      public FormMain getFormMain() {
         return mFormMain;
      }

      public bool deinitVRLib(VR.Result.Destroy.If callback) {
         return VR.destroyAsync(callback, getHandler(), null);
      }

      public void onVRLibDestroyed(bool success) {
         if (null != mFormDialog) {
            mFormDialog.Close();
         }
         Application.Exit();
      }

      private User.If mUser;

      public User.If getUser() {
         return mUser;
      }

      public void setUser(User.If user) {
         mUser = user;
      }

      private FormDialog mFormDialog;

      public FormDialog showDialog() {
         if (null == mFormDialog) {
            mFormDialog = new FormDialog();
         }
         mFormDialog.Show();
         return mFormDialog;
      }

      public FormDialog getFormDialog() {
         return mFormDialog;
      }

      public void onFormDialogClosed() {
         mFormDialog = null;
      }

      public void quit() {
         Application.Exit();
      }

      public SynchronizationContext getHandler() {
         return SynchronizationContext.Current;
      }

      private readonly EndPointConfigManager mEndPointCfgMgr = new EndPointConfigManager();

      public EndPointConfigManager getEndPointCfgMgr() {
         return mEndPointCfgMgr;
      }

      static public App getInstance() {
         if (null == sApp) {
            sApp = new App();
         }
         return sApp;
      }

   }
}
