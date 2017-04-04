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

   public partial class FormUILibLogin : FormSub {


      class UILibCallback : UILib.UILib.Callback {

         private readonly FormUILibLogin mFormLogin;

         public UILibCallback(FormUILibLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = Util.getLogTag(typeof(UILibCallback));

         public void onLibInitStatus(object closure, bool status) {
            if (status) {
               UILib.UILib.login();
            }
         }

         public void onLibDestroyStatus(object closure, bool status) {
         }

         public void onLoginSuccess(SDKLib.User.If user, object closure) {
            mFormLogin.mApp.setUser(user);
            mFormLogin.mApp.getFormMain().setControl(new FormLoggedIn());
         }

         public void onLoginFailure(object closure) {
         }

         public void showLoginUI(UserControl loginUI, object closure) {
            mFormLogin.showLogin(loginUI);
         }
      }

      private static readonly string TAG = Util.getLogTag(typeof(FormUILibLogin));

      private readonly EndPointConfigManager mEndPointCfgMgr;
      private readonly App mApp;
      private readonly UILibCallback mCallback;

      public FormUILibLogin() {

         InitializeComponent();

         mApp = App.getInstance();
         mEndPointCfgMgr = mApp.getEndPointCfgMgr();
         mCallback = new UILibCallback(this);
      }

      public override void onAdded() {
         ctrlWaitPanel.Visible = true;
         ctrlLoginPanel.Visible = false;

         mApp.mUILibCallback.mSubCallbacks.Add(mCallback);
         
         EndPointConfig config = mEndPointCfgMgr.getSelectedConfig();
         if (null == config) {
            ctrlEndPoint.Text = ResourceStrings.configureEndPoint;
         } else {
            ctrlEndPoint.Text = config.getUrl();
            UILib.UILib.init(mApp.getHandler(), config.getUrl(), config.getApiKey(), config.getSSOAppId(),
               config.getSSOAppSecret(),
               new HttpPlugin.RequestFactoryImpl(), mApp.mUILibCallback, null, null);
         }
      }

      public override void onRemoved() {
         mApp.mUILibCallback.mSubCallbacks.Remove(mCallback);
      }

      internal void showLogin(UserControl loginUI) {
         ctrlLoginPanel.Controls.Clear();
         ctrlLoginPanel.Controls.Add(loginUI);
         ctrlLoginPanel.Width = loginUI.Width;
         ctrlLoginPanel.Height = loginUI.Height;
         ctrlLoginPanel.Visible = true;
         ctrlWaitPanel.Visible = false;
      }

      private void ctrlEndPoint_Click(object sender, EventArgs e) {
         mApp.getFormMain().setControl(new FormEndPointConfig());
      }

      private void ctrlStatusMsg_Click(object sender, EventArgs e) {
         ctrlStatusMsg.Text = string.Empty;
      }

      private void FormUILibLogin_Load(object sender, EventArgs e) {

      }
   }
}
