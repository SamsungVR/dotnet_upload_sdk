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
            switch (status) {
               case VR.Result.Login.STATUS_MISSING_EMAIL_OR_PASSWORD:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrMissingEmailPassword;
                  break;
               case VR.Result.Login.STATUS_UNKNOWN_USER:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrUnknownUser;
                  break;
               case VR.Result.Login.STATUS_ACCOUNT_LOCKED_EXCESSIVE_FAILED_ATTEMPTS:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrExcessiveLoginAttempts;
                  break;
               case VR.Result.Login.STATUS_ACCOUNT_NOT_YET_ACTIVATED:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrPendingActivation;
                  break;
               case VR.Result.Login.STATUS_ACCOUNT_WILL_BE_LOCKED_OUT:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrAccountLocked;
                  break;
               case VR.Result.Login.STATUS_LOGIN_FAILED:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.vrLoginFailed;
                  break;
               default:
                  mFormLogin.ctrlLoginStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
                  break;
            }
            
         }

         public void onSuccess(object closure, User.If user) {
            Log.d(TAG, "login success");
            mFormLogin.mUILibImpl.onLoginSuccessInternal(user, true);
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
            mFormLogin.ctrlLoginStatus.Text = ex.ToString();
         }

      }

      class CallbackSSOLogin : SDKLib.VR.Result.LoginSSO.If {

         private readonly FormLogin mFormLogin;

         public CallbackSSOLogin(FormLogin formLogin) {
            mFormLogin = formLogin;
         }

         private static readonly string TAG = SDKLib.Log.getLogTag(typeof(CallbackSSOLogin));

         public void onFailure(object closure, int status) {
            Log.d(TAG, "login failed status: " + status);
            mFormLogin.toLoginPage();
            switch (status) {
               case VR.Result.LoginSSO.STATUS_AUTH_LOGIN_FAILED:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.ssoInvalidUserNameOrPassword;
                  break;
               case VR.Result.LoginSSO.STATUS_AUTH_VERIFY_FAILED:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.ssoUnableToVerifyAccount;
                  break;
               case VR.Result.LoginSSO.STATUS_REG_ACCOUNT_ALREADY_EXISTS:
                  mFormLogin.ctrlLoginStatus.Text = ResourceStrings.ssoVRAccountAlreadyCreated;
                  break;
               default:
                  mFormLogin.ctrlLoginStatus.Text = string.Format(ResourceStrings.failedWithStatus, status);
                  break;
            }
            
         }

         public void onSuccess(object closure, User.If user) {
            Log.d(TAG, "login succes sso");
            mFormLogin.mUILibImpl.onLoginSuccessInternal(user, true);
         }

         public void onCancelled(object closure) {
         }

         public void onException(object closure, Exception ex) {
            mFormLogin.ctrlLoginStatus.Text = ex.ToString();
         }

      }

      internal readonly string mSSOAppId, mSSOAppSecret;
      private readonly string mPostData;
      private readonly UILib.UILibImpl mUILibImpl;
      private static readonly string sLocalhost = "localhost";
      private static readonly Uri sLoginUri = new Uri("https://account.samsung.com/mobile/account/check.do");
      private readonly CallbackVRLogin mCallbackVRLogin;
      private readonly CallbackSSOLogin mCallbackSSOLogin;

      internal FormLogin(UILib.UILibImpl uiLibImpl, string ssoAppId, string ssoAppSecret) {
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

      internal void toLoginPage() {
         ctrlWebView.Visible = false;
         ctrlSSOProgress.Visible = true;
         ctrlVRPassword.Text = string.Empty;
         ctrlLoginStatus.Text = string.Empty;
         ctrlWebView.Navigate(sLoginUri, "", Encoding.UTF8.GetBytes(mPostData), 
            "Content-Type: application/x-www-form-urlencoded" + Environment.NewLine);
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
            ctrlWebView.Visible = true;
            ctrlSSOProgress.Visible = false;

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
                    "userId": "hkjkjkkuih",
                    "client_id": "uhjnjujj3k",
                    "api_server_url": "us-auth2.samsungosp.com",
                    "auth_server_url": "us-auth2.samsungosp.com",
                    "inputEmailID": "venkat230278+0@gmail.com"
                  }
                */
               Log.d(TAG, "Response json: " + json);
               string accessToken = Util.jsonGet<string>(json, "access_token");
               string authServerUrl = Util.jsonGet<string>(json, "auth_server_url");
               if (VR.loginSamsungAccount(accessToken, authServerUrl, mCallbackSSOLogin, UILib.sMainHandler, null)) {
                  ctrlSSOProgress.Visible = true;
                  ctrlWebView.Visible = false;
               } else {
                  toLoginPage();
                  ctrlLoginStatus.Text = ResourceStrings.failedToEnqueue;
               }
            }

         }
      }

      private void ctrlLogin_Click(object sender, EventArgs e) {
         string name = ctrlVRUsername.Text;
         string pwd = ctrlVRPassword.Text;
         if (VR.login(name, pwd, mCallbackVRLogin, UILib.sMainHandler, null)) {
         } else {
            ctrlLoginStatus.Text = ResourceStrings.failedToEnqueue;
         }
      }

      private void ctrlWebView_documentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e) {
         ctrlWebView.Visible = true;
         ctrlSSOProgress.Visible = false;
         //ctrlWebView.Document.Body.Style = "zoom:80%;";
      }

   }
}
