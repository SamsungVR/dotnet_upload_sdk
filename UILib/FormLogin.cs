using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using SDKLib;


namespace UILib {

   public partial class FormLogin : UserControl {

      class CallbackVRLogin : SDKLib.VR.Result.Login.If {

         private readonly FormLogin mFormLogin;

         public CallbackVRLogin(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = SDKLib.Log.getLogTag(typeof(CallbackVRLogin));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "login failed status: " + status);
         }

         public void onSuccess(object closure, User.If user) {
            Log.d(TAG, "login success");
            mFormLogin.mUILibImpl.onLoginSuccessInternal(user);
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
         }

      }

      class CallbackSSOLogin : SDKLib.VR.Result.LoginSSO.If {

         private readonly FormLogin mFormLogin;

         public CallbackSSOLogin(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = SDKLib.Log.getLogTag(typeof(CallbackVRLogin));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "login failed status: " + status);
         }

         public void onSuccess(object closure, User.If user) {
            Log.d(TAG, "login success");
            mFormLogin.mUILibImpl.onLoginSuccessInternal(user);
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
         }

      }

      internal readonly string mSSOAppId, mSSOAppSecret;
      private readonly string mPostData;
      private readonly UILibImpl mUILibImpl;
      private static readonly string sLocalhost = "localhost";
      private static readonly Uri sLoginUri = new Uri("https://account.samsung.com/mobile/account/check.do");
      private readonly CallbackVRLogin mCallbackVRLogin;
      private readonly CallbackSSOLogin mCallbackSSOLogin;

      internal FormLogin(UILibImpl uiLibImpl, string ssoAppId, string ssoAppSecret) {
         InitializeComponent();
         mUILibImpl = uiLibImpl;
         mSSOAppId = ssoAppId;
         mSSOAppSecret = ssoAppSecret;
         mPostData = string.Format("serviceID={0}&actionID=StartOAuth2&redirect_uri=http://{1}&accessToken=Y", mSSOAppId, sLocalhost);
         mCallbackVRLogin = new CallbackVRLogin(this);
         mCallbackSSOLogin = new CallbackSSOLogin(this);
      }

      private void ctrlShowPassword_CheckedChanged(object sender, EventArgs e) {
         ctrlVRPassword.UseSystemPasswordChar = !ctrlShowPassword.Checked;
      }

      private void toLoginPage() {
         ctrlWebView.Navigate(sLoginUri, "", Encoding.UTF8.GetBytes(mPostData), "Content-Type: application/x-www-form-urlencoded" + Environment.NewLine);
      }

      private void FormLogin_Load(object sender, EventArgs e) {
         toLoginPage();
      }

      private void ctrlButton_MouseUp(object sender, MouseEventArgs e) {
         ((Button)sender).ImageIndex = 0;
      }

      private void ctrlButton_MouseDown(object sender, MouseEventArgs e) {
         ((Button)sender).ImageIndex = 1;
      }

      private void ctrlGoHome_Click(object sender, EventArgs e) {
         toLoginPage();
      }

      private void ctrlgoBack_Click(object sender, EventArgs e) {
         if (ctrlWebView.CanGoBack) {
            ctrlWebView.GoBack();
         }
      }

      private static readonly string TAG = SDKLib.Util.getLogTag(typeof(FormLogin));

      private void ctrlWebView_onNavigating(object sender, WebBrowserNavigatingEventArgs e) {
         if (sLocalhost.Equals(e.Url.Authority)) {
            e.Cancel = true;

            HtmlElementCollection inputs = ctrlWebView.Document.GetElementsByTagName("input");
            JObject json = null;
            if (null != inputs && 1 == inputs.Count) {

               for (int i = inputs.Count - 1; i >= 0; i -= 1) {
                  if (!"code".Equals(inputs[i].Name)) {
                     continue;
                  }
                  try {
                     json = JObject.Parse(inputs[i].GetAttribute("value"));
                  } catch (Exception ex) {
                     SDKLib.Log.d(TAG, inputs[i].Id + " " + inputs[i].Name + " " + ex);
                     json = null;
                  }
               }
            }
            if (null == json) {
               toLoginPage();
            } else {

               /*
                 {
                    "access_token": "kjdkdkdjdl",
                    "token_type": "bearer",
                    "access_token_expires_in": 530184,
                    "refresh_token": "-1",
                    "refresh_token_expires_in": -1,
                    "userId": "jspwny4nuh",
                    "client_id": "2269tcup3k",
                    "api_server_url": "us-auth2.samsungosp.com",
                    "auth_server_url": "us-auth2.samsungosp.com",
                    "inputEmailID": "venkat230278+0@gmail.com"
                  }
                */
               string accessToken = Util.jsonGet<string>(json, "access_token");
               string authServerUrl = Util.jsonGet<string>(json, "auth_server_url");
               VR.loginSamsungAccount(accessToken, authServerUrl, mCallbackSSOLogin, mUILibImpl.mUIHandler, null);
            }

         }
      }

      private void ctrlLogin_Click(object sender, EventArgs e) {
         string name = ctrlVRUsername.Text;
         string pwd = ctrlVRPassword.Text;
         if (VR.login(name, pwd, mCallbackVRLogin, mUILibImpl.mUIHandler, null)) {
         } else {
         }
      }

   }
}
