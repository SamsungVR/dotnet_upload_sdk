using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SDKLib;
using UILib;

namespace SampleApp {

   public partial class FormLogin : UserControl {

      private static readonly string TAG = Util.getLogTag(typeof(FormLogin));

      class CallbackDestroy : VR.Result.Destroy.If {

         private readonly FormLogin mFormLogin;

         public CallbackDestroy(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackDestroy));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "destroy failed status: " + status);
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "destroy success");
            App app = App.getInstance();

            if (mFormLogin.initVRLib()) {
               mFormLogin.ctrlStatusMsg.Text = ResourceStrings.initVR;
            } else {
               mFormLogin.ctrlStatusMsg.Text = ResourceStrings.initVRFailed;
            }
         }

      }

      class CallbackLogin : VR.Result.Login.If {

         private readonly FormLogin mFormLogin;

         public CallbackLogin(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackLogin));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "login failed status: " + status);
            reallowLogin(string.Format(ResourceStrings.failedWithStatus, status));
         }

         public void onSuccess(object closure, User.If user) {
            Log.d(TAG, "login success");
            mFormLogin.ctrlStatusMsg.Text = string.Format(ResourceStrings.loggedInAs, user.getName());
            mFormLogin.mApp.setUser(user);
            mFormLogin.mApp.getFormMain().setControl(new FormLoggedIn());
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
            reallowLogin(ex.Message);
         }

         private void reallowLogin(string msg) {
            mFormLogin.ctrlStatusMsg.Text = msg;
            mFormLogin.ctrlCredsGroup.Enabled = true;
         }
      }

      class CallbackInit : VR.Result.Init.If {

         private readonly FormLogin mFormLogin;

         public CallbackInit(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = Util.getLogTag(typeof(CallbackInit));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "init failed status: " + status);
         }

         public void onSuccess(object closure) {
            Log.d(TAG, "init success");
            mFormLogin.ctrlStatusMsg.Text = ResourceStrings.initVRSuccess;
            mFormLogin.ctrlCredsGroup.Enabled = true;
         }
      }

      private readonly CallbackDestroy mCallbackDestroy;
      private readonly CallbackInit mCallbackInit;
      private readonly CallbackLogin mCallbackLogin;
      private readonly EndPointConfigManager mEndPointCfgMgr;
      private readonly App mApp;

      public FormLogin() {

         InitializeComponent();

         mApp = App.getInstance();

         /*
          * All callbacks are weak refs in SDK. We MUST maintain hard refs
          */

         mCallbackDestroy = new CallbackDestroy(this);
         mCallbackInit = new CallbackInit(this);
         mCallbackLogin = new CallbackLogin(this);

         mEndPointCfgMgr = mApp.getEndPointCfgMgr();
         EndPointConfig config = mEndPointCfgMgr.getSelectedConfig();

         ctrlCredsGroup.Enabled = false;

         if (null != config) {
            ctrlEndPoint.Text = config.getUrl();

            if (VR.destroyAsync(mCallbackDestroy, mApp.getHandler(), null)) {
               ctrlStatusMsg.Text = ResourceStrings.deinitVR;
            } else {
               initVRLib();
            }
         } else {
            ctrlEndPoint.Text = ResourceStrings.configureEndPoint;
         }
      }

      private bool initVRLib() {
         EndPointConfig config = mEndPointCfgMgr.getSelectedConfig();
         if (null == config) {
            return false;
         }
         if (VR.initAsync(config.getUrl(), config.getApiKey(), new HttpPlugin.RequestFactoryImpl(), new CallbackInit(this), mApp.getHandler(), null)) {
            return true;
         }
         return false;
      }

      private void ctrlLogin_Click(object sender, EventArgs e) {
         string name = ctrlUsername.Text;
         string pwd = ctrlPassword.Text;
         if (VR.login(name, pwd, mCallbackLogin, App.getInstance().getHandler(), null)) {
            ctrlStatusMsg.Text = ResourceStrings.requestInProgress;
            ctrlCredsGroup.Enabled = false;
         } else {
            ctrlStatusMsg.Text = ResourceStrings.unableToQueueRequest;
         }
      }

      private void ctrlEndPoint_Click(object sender, EventArgs e) {
         mApp.getFormMain().setControl(new FormEndPointConfig());
      }

      private void ctrlStatusMsg_Click(object sender, EventArgs e) {
         ctrlStatusMsg.Text = string.Empty;
      }

      private void FormLogin_Load(object sender, EventArgs e) {

      }

      private void button1_Click(object sender, EventArgs e) {
         UILib.UILib.login();
      }

      private void ctrlCredsGroup_Enter(object sender, EventArgs e) {

      }
   }
}
