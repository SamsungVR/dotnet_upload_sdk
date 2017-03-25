using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;


namespace UILib {

   public partial class FormLogin : UserControl {

      internal readonly string mSSOAppId, mSSOAppSecret;
      private readonly string mPostData;

      private static readonly string sLocalhost = "localhost";
      private static readonly Uri sLoginUri = new Uri("https://account.samsung.com/mobile/account/check.do");

      public FormLogin(string ssoAppId, string ssoAppSecret) {
         InitializeComponent();
         mSSOAppId = ssoAppId;
         mSSOAppSecret = ssoAppSecret;
         mPostData = string.Format("serviceID={0}&actionID=StartOAuth2&redirect_uri=http://{1}&accessToken=Y", mSSOAppId, sLocalhost);

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
               SDKLib.Log.d(TAG, "Creds: " + json);
            }

         }
      }

   }
}
