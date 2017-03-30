﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using SDKLib;
using UILib;

namespace SampleApp {

   public partial class FormUILibLogin : UserControl {


      class UILibCallback : UILib.UILib.Callback {

         private readonly FormUILibLogin mFormLogin;

         public UILibCallback(FormUILibLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = Util.getLogTag(typeof(UILibCallback));

         public void onLibInitStatus(object closure, bool status) {
            Log.d(TAG, "onLibInitStatus " + status);
            if (status) {
               UILib.UILib.login();
            }
         }

         public void onLibDestroyStatus(object closure, bool status) {
            Log.d(TAG, "onLibDestroyStatus " + status);
         }

         public void onLoginSuccess(SDKLib.User.If user, object closure) {
            Log.d(TAG, "onLoggedIn " + user.getName());
            mFormLogin.mApp.setUser(user);
            mFormLogin.mApp.getFormMain().setControl(new FormLoggedIn());
         }

         public void onLoginFailure(object closure) {
         }

         public void showLoginUI(UserControl loginUI, object closure) {
            Log.d(TAG, "showLoginUI");
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
         initVRLib();
      }

      internal void showLogin(UserControl loginUI) {
         ctrlLoginPanel.Controls.Clear();
         ctrlLoginPanel.Controls.Add(loginUI);
         ctrlLoginPanel.Width = loginUI.Width;
         ctrlLoginPanel.Height = loginUI.Height;
      }

      private bool initVRLib() {
         EndPointConfig config = mEndPointCfgMgr.getSelectedConfig();
         if (null == config) {
            ctrlEndPoint.Text = ResourceStrings.configureEndPoint;
            return false;
         }
         ctrlEndPoint.Text = config.getUrl();
         return UILib.UILib.init(mApp.getHandler(), config.getUrl(), config.getApiKey(), config.getSSOAppId(), 
            config.getSSOAppSecret(),
            new HttpPlugin.RequestFactoryImpl(), mCallback, null, null);
      }

      private void ctrlEndPoint_Click(object sender, EventArgs e) {
         mApp.getFormMain().setControl(new FormEndPointConfig());
      }

      private void ctrlStatusMsg_Click(object sender, EventArgs e) {
         ctrlStatusMsg.Text = string.Empty;
      }
   }
}
