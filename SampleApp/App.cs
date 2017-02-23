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

      private static readonly string TAG = Util.getLogTag(typeof(App));
      private FormMain mFormMain;

      public class MyMessageFilter : IMessageFilter {

         public bool PreFilterMessage(ref Message m) {
            switch (m.Msg) {
               case 0xF: /* WM_PAINT */
               case 0xA0:
               case 0x200:
               case 0x113:
               case 0x202:
               case 0x2a3: /* WM_MOUSELEAVE */
               case 0x201: /* WM_LBUTTONDOWN */
               case 0x05: /* WM_SIZE */
               case 0x400: /* WM_USER */
               case 0X2A1: /* WM_MOUSEHOVER */
               case 0x100: /* WM_KEYDOWN */
               case 0x101: /* WM_KEYUP */
               case 0x8002: /* DBT_DEVICEQUERYREMOVEFAILED */
               case 0x8003: /* DBT_DEVICEREMOVEPENDING */
                  break;
               default:
                  Log.d(App.TAG, m.ToString());
                  break;
            }
            return false;
         }
      }

      public void main() {
         Application.EnableVisualStyles();
         WindowsFormsSynchronizationContext handler = new WindowsFormsSynchronizationContext();
         SynchronizationContext.SetSynchronizationContext(handler);
         mFormMain = new FormMain();
         mFormMain.Show();
         mFormMain.setControl(new FormLogin());
         //Application.AddMessageFilter(new MyMessageFilter());
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
